using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Toolchains.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Performance.Application;
using Performance.API.Controllers;
using Performance.Domain;
using Performance.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Performance.Application.Interface.Repository;
using Performance.Infrastructure.Repositories;
using Performance.Application.DTO;
using Performance.Application.Common;

namespace BenchmarkSuite
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    [MinColumn, MaxColumn]
    [MarkdownExporter]
    [SimpleJob(warmupCount: 2, iterationCount: 20, invocationCount: 1)]
    public class Benchmarks
    {
        private IConfigurationRoot _configuration;
        private const int PageSize = 50;
        private const int NoOfConcurrentUsers = 1000;

        private int _totalRecords;
        private int _totalPages;
        private Random _random;

        private ServiceProvider _serviceProvider;
        private IServiceScope _scope;
        private IOffsetRepository _offsetRepo;
        private ICursorRepository _cursorRepo;
        private ILogger<UserController> _logger;

        [GlobalSetup]
        public async Task Setup()
        {
            _random = new Random(42);

            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets<Benchmarks>()
                .Build();

            var services = BuildServiceCollection();
            using (var serviceProvider = services.BuildServiceProvider())
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
                _totalRecords = await dbContext.Users.CountAsync();
            }

            _totalPages = (int)Math.Ceiling((double)_totalRecords / PageSize);

            Console.WriteLine($"Database has {_totalRecords} records.");
            Console.WriteLine($"Total pages: {_totalPages}");
            Console.WriteLine($"Page size: {PageSize}");
        }

        [IterationSetup]
        public void IterationSetup()
        {
            var services = BuildServiceCollection();
            _serviceProvider = services.BuildServiceProvider();

            _scope = _serviceProvider.CreateScope();
            _offsetRepo = _scope.ServiceProvider.GetRequiredService<IOffsetRepository>();
            _cursorRepo = _scope.ServiceProvider.GetRequiredService<ICursorRepository>();
            _logger = _scope.ServiceProvider.GetRequiredService<ILogger<UserController>>();
        }

        [IterationCleanup] // optional, but important for result consistency
        public void IterationCleanup()
        {
            _scope?.Dispose();
            _offsetRepo = null;
            _cursorRepo = null;
            _logger = null;

            _serviceProvider?.Dispose();
            _serviceProvider = null;
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            // Final cleanup
        }

        private ServiceCollection BuildServiceCollection()
        {
            var services = new ServiceCollection();

            services.AddDbContextPool<UserDbContext>(options =>
                options.UseSqlServer(
                    _configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(10),
                            errorNumbersToAdd: null
                        );
                        sqlOptions.CommandTimeout(120);
                    }
                )
            );

            services.AddScoped<IOffsetRepository, OffsetRepository>();
            services.AddScoped<ICursorRepository, CursorRepository>();

            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Warning);
            });

            return services;
        }

        #region Helper Methods

        private async Task<IActionResult> ExecuteSingleRequest(UserRequestDTO request)
        {
            var controller = new UserController(_offsetRepo, _cursorRepo, _logger);
            return await controller.GetPaginatedUsers(request);
        }

        private async Task<object> ExecuteConcurrentRequests(Func<int, UserRequestDTO> requestFactory)
        {
            var tasks = new List<Task<RequestResult>>();
            var stopwatch = Stopwatch.StartNew();

            // Use the service provider from IterationSetup
            for (int i = 0; i < NoOfConcurrentUsers; i++)
            {
                int userIndex = i;
                tasks.Add(Task.Run(async () =>
                {
                    var userStopwatch = Stopwatch.StartNew();
                    try
                    {
                        // Create a new scope for each user to ensure thread safety
                        using var scope = _serviceProvider.CreateScope();
                        var offsetRepo = scope.ServiceProvider.GetRequiredService<IOffsetRepository>();
                        var cursorRepo = scope.ServiceProvider.GetRequiredService<ICursorRepository>();
                        var logger = scope.ServiceProvider.GetRequiredService<ILogger<UserController>>();
                        var controller = new UserController(offsetRepo, cursorRepo, logger);

                        var request = requestFactory(userIndex);
                        var result = await controller.GetPaginatedUsers(request);

                        userStopwatch.Stop();
                        return new RequestResult
                        {
                            Success = true,
                            Duration = userStopwatch.ElapsedMilliseconds,
                            UserIndex = userIndex
                        };
                    }
                    catch (Exception ex)
                    {
                        userStopwatch.Stop();
                        return new RequestResult
                        {
                            Success = false,
                            Duration = userStopwatch.ElapsedMilliseconds,
                            UserIndex = userIndex,
                            Error = ex.Message
                        };
                    }
                }));
            }

            var results = await Task.WhenAll(tasks);
            stopwatch.Stop();

            // Don't dispose service provider here - IterationCleanup handles it

            // Calculate and log statistics
            var successCount = results.Count(r => r.Success);
            var failCount = results.Count(r => !r.Success);
            var avgDuration = results.Average(r => r.Duration);
            var minDuration = results.Min(r => r.Duration);
            var maxDuration = results.Max(r => r.Duration);

            var sortedDurations = results.OrderBy(r => r.Duration).ToList();
            var p50 = sortedDurations[sortedDurations.Count / 2].Duration;
            var p95 = sortedDurations[(int)(sortedDurations.Count * 0.95)].Duration;
            var p99 = sortedDurations[(int)(sortedDurations.Count * 0.99)].Duration;

            Console.WriteLine($"\n--- Concurrent Request Statistics ({NoOfConcurrentUsers} users) ---");
            Console.WriteLine($"Total Time: {stopwatch.ElapsedMilliseconds}ms ({stopwatch.ElapsedMilliseconds / 1000.0:F2}s)");
            Console.WriteLine($"Success: {successCount}, Failed: {failCount}");
            Console.WriteLine($"Success Rate: {(successCount / (double)results.Length * 100):F2}%");
            Console.WriteLine($"Response Times (ms):");
            Console.WriteLine($"  Min: {minDuration}");
            Console.WriteLine($"  Avg: {avgDuration:F2}");
            Console.WriteLine($"  Max: {maxDuration}");
            Console.WriteLine($"  P50: {p50}");
            Console.WriteLine($"  P95: {p95}");
            Console.WriteLine($"  P99: {p99}");
            Console.WriteLine($"Throughput: {(NoOfConcurrentUsers / (stopwatch.ElapsedMilliseconds / 1000.0)):F2} req/sec");

            if (failCount > 0)
            {
                Console.WriteLine($"\nErrors:");
                var errorGroups = results.Where(r => !r.Success)
                    .GroupBy(r => r.Error)
                    .OrderByDescending(g => g.Count());

                foreach (var group in errorGroups.Take(3))
                {
                    Console.WriteLine($"  {group.Key}: {group.Count()} times");
                }
            }

            return results;
        }

        private class RequestResult
        {
            public bool Success { get; set; }
            public long Duration { get; set; }
            public int UserIndex { get; set; }
            public string Error { get; set; }
        }

        #endregion

        #region Offset Pagination Benchmarks

        [Benchmark(Description = "Offset: First Page")]
        public async Task<IActionResult> Offset_RetrieveFirstPage()
        {
            var request = new UserRequestDTO
            {
                PaginationType = (int)PaginationType.Offset,
                offsetPagination = new OffsetPaginationRequest
                {
                    Page = 1,
                    PageSize = PageSize
                }
            };
            return await ExecuteSingleRequest(request);
        }

        [Benchmark(Description = "Offset: Middle Page")]
        public async Task<IActionResult> Offset_RetrieveRandomPage()
        {
            var request = new UserRequestDTO
            {
                PaginationType = (int)PaginationType.Offset,
                offsetPagination = new OffsetPaginationRequest
                {
                    Page = 1000,
                    PageSize = PageSize
                }
            };
            return await ExecuteSingleRequest(request);
        }

        [Benchmark(Description = "Offset: Last Page")]
        public async Task<IActionResult> Offset_RetrieveLastPage()
        {
            var request = new UserRequestDTO
            {
                PaginationType = (int)PaginationType.Offset,
                offsetPagination = new OffsetPaginationRequest
                {
                    Page = 2000,
                    PageSize = PageSize
                }
            };
            return await ExecuteSingleRequest(request);
        }

        #endregion

        #region Cursor Pagination Benchmarks

        [Benchmark(Description = "Cursor: First Page")]
        public async Task<IActionResult> Cursor_RetrieveFirstPage()
        {
            var request = new UserRequestDTO
            {
                PaginationType = (int)PaginationType.Cursor,
                cursorPagination = new CursorPaginationRequest
                {
                    Cursor = 0,
                    PageSize = PageSize
                }
            };
            return await ExecuteSingleRequest(request);
        }

        [Benchmark(Description = "Cursor: Middle Page")]
        public async Task<IActionResult> Cursor_RetrieveRandomPage()
        {
            var request = new UserRequestDTO
            {
                PaginationType = (int)PaginationType.Cursor,
                cursorPagination = new CursorPaginationRequest
                {
                    Cursor = 50000,
                    PageSize = PageSize
                }
            };
            return await ExecuteSingleRequest(request);
        }

        [Benchmark(Description = "Cursor: Last Page")]
        public async Task<IActionResult> Cursor_RetrieveLastPage()
        {
            var request = new UserRequestDTO
            {
                PaginationType = (int)PaginationType.Cursor,
                cursorPagination = new CursorPaginationRequest
                {
                    Cursor = 99950,
                    PageSize = PageSize
                }
            };
            return await ExecuteSingleRequest(request);
        }

        #endregion

        #region Concurrent Calls

        [Benchmark(Description = "Concurrent Offset: Mixed Pages")]
        public async Task<object> Offset_RetrieveMixedPages()
        {
            return await ExecuteConcurrentRequests((int userIndex) =>
            {
                // 40% first page, 30% pages 2-10, 20% middle pages, 10% last pages
                int page;
                var distribution = _random.Next(100);

                if (distribution < 40)
                    page = 1;
                else if (distribution < 70)
                    page = _random.Next(2, Math.Min(11, _totalPages + 1));
                else if (distribution < 90)
                    page = _random.Next(11, Math.Max(12, _totalPages / 2));
                else
                    page = _random.Next(Math.Max(1, _totalPages - 10), _totalPages + 1);

                return new UserRequestDTO
                {
                    PaginationType = (int)PaginationType.Offset,
                    offsetPagination = new OffsetPaginationRequest
                    {
                        Page = page,
                        PageSize = PageSize
                    }
                };
            });
        }

        [Benchmark(Description = "Concurrent Cursor: Mixed Pages")]
        public async Task<object> Cursor_RetrieveMixedPages()
        {
            return await ExecuteConcurrentRequests((int userIndex) =>
            {
                // 40% first page, 30% early cursors, 20% middle cursors, 10% late cursors
                long cursor;
                var distribution = _random.Next(100);

                if (distribution < 40)
                    cursor = 0; // First page
                else if (distribution < 70)
                    cursor = _random.Next(PageSize, Math.Min(PageSize * 10, _totalRecords - PageSize));
                else if (distribution < 90)
                    cursor = _random.Next(PageSize * 10, Math.Max(PageSize * 11, _totalRecords / 2));
                else
                    cursor = _random.Next(Math.Max(0, _totalRecords - 500), _totalRecords - PageSize);

                return new UserRequestDTO
                {
                    PaginationType = (int)PaginationType.Cursor,
                    cursorPagination = new CursorPaginationRequest
                    {
                        Cursor = cursor,
                        PageSize = PageSize
                    }
                };
            });
        }

        [Benchmark(Description = "Concurrent: 50/50 Mixed Traffic")]
        public async Task<object> Mixed_OffsetAndCursor_5050()
        {
            return await ExecuteConcurrentRequests((int userIndex) =>
            {
                bool useOffset = _random.Next(2) == 0;

                if (useOffset)
                {
                    return new UserRequestDTO
                    {
                        PaginationType = (int)PaginationType.Offset,
                        offsetPagination = new OffsetPaginationRequest
                        {
                            Page = _random.Next(1, Math.Min(100, _totalPages + 1)),
                            PageSize = PageSize
                        }
                    };
                }
                else
                {
                    return new UserRequestDTO
                    {
                        PaginationType = (int)PaginationType.Cursor,
                        cursorPagination = new CursorPaginationRequest
                        {
                            Cursor = _random.Next(0, Math.Max(0, _totalRecords - PageSize)),
                            PageSize = PageSize
                        }
                    };
                }
            });
        }

        #endregion
    }
}