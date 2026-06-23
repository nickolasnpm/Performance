using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Performance.Application.Common.Models;
using Performance.Application.Common.Settings;
using Performance.Application.DTOs;
using Performance.Application.Extensions.Repository.EntityIncludeOptions;
using Performance.Application.Extensions.Repository.EntityColumnSort;
using Performance.Application.Interface.Repository;
using Performance.Domain.Entity;
using Performance.Infrastructure.Caching;

namespace Performance.Infrastructure.Persistence.Repositories
{
    public class UserRepositories(PerformanceDbContext context, IOptions<CacheSettings> cacheSettings)
        : IUserRepositories
    {
        public IQueryable<User> GetAll()
        {
            return context.Users.AsNoTracking();
        }

        public async Task<PaginatedResult<User>> GetPaginatedUsersByOffset(OffsetPaginationRequest request, UserIncludeOptions includeOptions)
        {
            IQueryable<User> queryable = GetAll();

            var totalCount = await GetCachedUserCount(queryable);

            if (!string.IsNullOrWhiteSpace(request.Search) && request.Search.Length >= 3)
            {
                var search = request.Search.Trim() + "%";

                queryable = queryable.Where(u =>
                    EF.Functions.Like(u.Username, search) ||
                    EF.Functions.Like(u.Email, search) ||
                    EF.Functions.Like(u.FirstName, search) ||
                    EF.Functions.Like(u.LastName, search));
            }

            if (includeOptions == UserIncludeOptions.All)
            {
                queryable = queryable.AsSplitQuery();
            }

            queryable = queryable.ApplyIncludes(includeOptions);
            queryable = queryable.ApplySorting(request.SortBy, request.IsAscending);

            return new PaginatedResult<User>(
                Items: queryable.Skip((request.Page! - 1) * request.Size).Take(request.Size),
                TotalCount: totalCount);
        }

        public async Task<PaginatedResult<User>> GetPaginatedUsersByCursor(long cursorValue, CursorPaginationRequest request, UserIncludeOptions includeOptions)
        {
            IQueryable<User> queryable = GetAll();

            int totalCount = await GetCachedUserCount(queryable);

            if (request.IsQueryPreviousPage)
            {
                queryable = queryable.Where(u => u.Id < cursorValue).OrderByDescending(u => u.Id);
            }
            else
            {
                queryable = queryable.Where(u => u.Id > cursorValue).OrderBy(u => u.Id);
            }

            if (includeOptions == UserIncludeOptions.All)
            {
                queryable = queryable.AsSplitQuery();
            }

            queryable = queryable.ApplyIncludes(includeOptions);

            return new PaginatedResult<User>(
                Items: queryable.Take(request.Size + 1),
                TotalCount: totalCount);
        }

        public async Task Create(IEnumerable<User> entities)
        {
            await context.BulkInsertAsync(entities);
        }

        public async Task Create(User user)
        {
            await context.Users.AddAsync(user);
        }

        public async Task Update(IEnumerable<User> entities)
        {
            await context.BulkUpdateAsync(entities);
        }

        public async Task Delete(HashSet<long> ids)
        {
            await context.Users.Where(u => ids.Contains(u.Id)).ExecuteDeleteAsync();
        }

        private async Task<int> GetCachedUserCount(IQueryable<User> queryable)
        {
            if (cacheSettings.Value.Enabled)
            {
                return await AsyncCache<int>.GetOrUpdateAsync(nameof(cacheSettings.Value.UserCount),
                    TimeSpan.FromMinutes(cacheSettings.Value.UserCount.ExpirationMinutes), () => queryable.CountAsync());
            }
            else
            {
                return await queryable.CountAsync();
            }
        }
    }
}
