using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Performance.API.Controllers;
using Performance.Application.Common.Enums;
using Performance.Application.Common.Models;
using Performance.Application.Common.Settings;
using Performance.Application.DTOs;
using Performance.Application.DTOs.Users;
using Performance.Application.Interface.Repository;
using Performance.Application.Interface.Security;
using Performance.Application.Interface.Services;
using Performance.Application.Interface.UnitOfWork;
using Performance.Application.Services;
using Performance.Infrastructure.Persistence;
using Performance.Infrastructure.Persistence.Repositories;
using Performance.Infrastructure.Persistence.UnitOfWork;
using Performance.Infrastructure.Security;
using System.Diagnostics;

namespace BenchmarkSuite
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    [MinColumn, MaxColumn]
    [MarkdownExporter]
    [SimpleJob(warmupCount: 1, iterationCount: 5, invocationCount: 1)]
    public class Benchmarks
    {
        private IConfigurationRoot? _configuration;
        private const int PageSize = 50;
        private const int NoOfConcurrentUsers = 1000;

        private int _totalRecords;
        private int _totalPages;
        private Random? _random;

        private ServiceProvider? _serviceProvider;
        private IServiceScope? _scope;

        private IUserServices? _userServices;

        private ILogger<UserController>? _logger;

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

            var key = _configuration["IdEncryptionSettings:Key"];
            Console.WriteLine($"Key loaded: {(string.IsNullOrEmpty(key) ? "NO - KEY IS NULL/EMPTY" : "YES")}");

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

            _userServices = _scope.ServiceProvider.GetRequiredService<IUserServices>();
            //_unitOfWork = _scope.ServiceProvider.GetService<IUnitOfWork>();
            //_userRepositories = _scope.ServiceProvider.GetService<IUserRepositories>();

            _logger = _scope.ServiceProvider.GetRequiredService<ILogger<UserController>>();
        }

        [IterationCleanup] // optional, but important for result consistency
        public void IterationCleanup()
        {
            _scope?.Dispose();
            _userServices = null;
            //_unitOfWork?.Dispose();
            //_userRepositories = null;
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

            // Pass _configuration directly instead of rebuilding
            services.Configure<IdEncryptionSettings>(
                _configuration!.GetSection("IdEncryptionSettings")
            );

            services.AddDbContextPool<UserDbContext>(options =>
                options.UseSqlServer(
                    _configuration?.GetConnectionString("DefaultConnection"),
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

            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepositories, UserRepositories>();
            services.AddScoped<IIdHelper, IdHelper>();

            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Warning);
            });

            return services;
        }

        #region Helper Methods

        private async Task<ActionResult<ListResponseDTO<UserDTO>>> ExecuteSingleRequest(ListRequestDTO request)
        {
            var controller = new UserController(_userServices!);
            return await controller.GetPaginatedUsers(request);
        }

        private class RequestResult
        {
            public bool Success { get; set; }
            public long Duration { get; set; }
            public int UserIndex { get; set; }
            public string? Error { get; set; }
        }

        #endregion

        #region Offset Pagination Benchmarks

        [Benchmark(Description = "Offset: First Page")]
        public async Task<ActionResult<ListResponseDTO<UserDTO>>> Offset_RetrieveFirstPage()
        {
            var request = new ListRequestDTO
            (
                PaginationType: PaginationType.Offset,
                OffsetPagination: new OffsetPaginationRequest
                (
                    Page: 1,
                    Size: PageSize
                ),
                CursorPagination: null
            );
            return await ExecuteSingleRequest(request);
        }

        [Benchmark(Description = "Offset: Middle Page")]
        public async Task<ActionResult<ListResponseDTO<UserDTO>>> Offset_RetrieveRandomPage()
        {
            var request = new ListRequestDTO
            (
                PaginationType: PaginationType.Offset,
                OffsetPagination: new OffsetPaginationRequest
                (
                    Page: 1000,
                    Size: PageSize
                ),
                CursorPagination: null
            );
            return await ExecuteSingleRequest(request);
        }

        [Benchmark(Description = "Offset: Last Page")]
        public async Task<ActionResult<ListResponseDTO<UserDTO>>> Offset_RetrieveLastPage()
        {
            var request = new ListRequestDTO
            (
                PaginationType: PaginationType.Offset,
                OffsetPagination: new OffsetPaginationRequest
                (
                    Page: 2000,
                    Size: PageSize
                ),
                CursorPagination: null
            );
            return await ExecuteSingleRequest(request);
        }

        #endregion

        #region Cursor Pagination Benchmarks

        [Benchmark(Description = "Cursor: Similar Across Pages")]
        public async Task<ActionResult<ListResponseDTO<UserDTO>>> Cursor_RetrieveFirstPage()
        {
            var request = new ListRequestDTO
            (
                PaginationType: PaginationType.Cursor,
                OffsetPagination: null,
                CursorPagination: new CursorPaginationRequest
                (
                    Cursor: null,
                    IsQueryPreviousPage: false,
                    Size: PageSize
                )
            );
            return await ExecuteSingleRequest(request);
        }

        #endregion
    }
}