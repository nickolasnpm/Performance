using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Performance.Application.Common.Models;
using Performance.Application.Common.Settings;
using Performance.Application.DTOs;
using Performance.Application.Extensions.Repository;
using Performance.Application.Extensions.Repository.EntityIncludeOptions;
using Performance.Application.Interface.Repository;
using Performance.Domain.Entity;
using Performance.Infrastructure.Caching;

namespace Performance.Infrastructure.Persistence.Repositories
{
    public class UserRepositories (UserDbContext userDbContext, IOptions<AppSettings> appSettings, IOptions<CacheSettings> cacheSettings)
        : IUserRepositories
    {
        public IQueryable<User> GetAll()
        {
            return userDbContext.Users.AsNoTracking();
        }

        public async Task<PaginatedResult<User>> GetPaginatedUsersByOffset(OffsetPaginationRequest request, UserIncludeOptions includeOptions)
        {
            IQueryable<User> queryable = GetAll();

            var totalCount = 0;

            if (appSettings.Value.IsUseCache)
            {
                var userCountCacheSettings = cacheSettings.Value.Items[CacheKeys.UserCount];

                totalCount = await AsyncCache<int>.GetOrUpdateAsync(userCountCacheSettings.Key,
                    TimeSpan.FromMinutes(userCountCacheSettings.ExpirationMinutes), () => queryable.CountAsync());
            }
            else
            {
                totalCount = await queryable.CountAsync();
            }

            if (includeOptions == UserIncludeOptions.All)
            {
                queryable = queryable.AsSplitQuery();
            }

            queryable = queryable.ApplyIncludes(includeOptions);

            return new PaginatedResult<User>(
                Items: queryable.OrderBy(u => u.Id).Skip((request.Page! - 1) * request.Size).Take(request.Size), 
                TotalCount: totalCount);
        }

        public async Task<PaginatedResult<User>> GetPaginatedUsersByCursor(long cursorValue, CursorPaginationRequest request, UserIncludeOptions includeOptions)
        {
            IQueryable<User> queryable = GetAll();

            int totalCount = 0;

            if (appSettings.Value.IsUseCache)
            {
                var userCountCacheSettings = cacheSettings.Value.Items[CacheKeys.UserCount];

                totalCount = await AsyncCache<int>.GetOrUpdateAsync(userCountCacheSettings.Key,
                    TimeSpan.FromMinutes(userCountCacheSettings.ExpirationMinutes), () => queryable.CountAsync());
            }
            else
            {
                totalCount = await queryable.CountAsync();
            }

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

        public async Task<List<User>> Create(List<User> entities)
        {
            await userDbContext.AddRangeAsync(entities);
            return entities;
        }

        public async Task<bool> Delete(HashSet<long> ids)
        {
            await userDbContext.Users.Where(u => ids.Contains(u.Id)).ExecuteDeleteAsync();
            return true;
        }
    }
}
