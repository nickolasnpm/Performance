using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Performance.Application.Configuration;
using Performance.Application.DTO;
using Performance.Application.Extensions.Repository;
using Performance.Application.Interface.Repository;
using Performance.Application.Queries;
using Performance.Domain.Entity;
using Performance.Infrastructure.Caching;

namespace Performance.Infrastructure.Repositories
{
    public class UserRepositories (UserDbContext _userDbContext, IOptions<AppSettings> _appSettings, IOptions<CacheSettings> _cacheSettings)
        : IUserRepositories
    {
        public IQueryable<User> GetAll()
        {
            return _userDbContext.Users.AsNoTracking();
        }

        public async Task<(IQueryable<User> Items, int TotalCount)> GetPaginatedUsersByOffset(OffsetPaginationRequest request, UserIncludeOptions includeOptions)
        {
            IQueryable<User> queryable = GetAll();

            var totalCount = 0;

            if (_appSettings.Value.IsUseCache)
            {
                var userCountCacheSettings = _cacheSettings.Value.Items[CacheKeys.UserCount];

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

            return (queryable.OrderBy(u => u.Id).Skip((request.Page! - 1) * request.PageSize).Take(request.PageSize), totalCount);
        }

        public async Task<(IQueryable<User> Items, int TotalCount)> GetPaginatedUsersByCursor(CursorPaginationRequest request, UserIncludeOptions includeOptions)
        {
            IQueryable<User> queryable = GetAll();

            int totalCount = 0;

            if (_appSettings.Value.IsUseCache)
            {
                var userCountCacheSettings = _cacheSettings.Value.Items[CacheKeys.UserCount];

                totalCount = await AsyncCache<int>.GetOrUpdateAsync(userCountCacheSettings.Key,
                    TimeSpan.FromMinutes(userCountCacheSettings.ExpirationMinutes), () => queryable.CountAsync());
            }
            else
            {
                totalCount = await queryable.CountAsync();
            }

            if (request.IsQueryPreviousPage)
            {
                queryable = queryable.Where(u => u.Id < request.Cursor).OrderByDescending(u => u.Id);
            }
            else
            {
                queryable = queryable.Where(u => u.Id > request.Cursor).OrderBy(u => u.Id);
            }

            if (includeOptions == UserIncludeOptions.All)
            {
                queryable = queryable.AsSplitQuery();
            }

            queryable = queryable.ApplyIncludes(includeOptions);

            return (queryable.Take(request.PageSize + 1), totalCount);
        }

    }
}
