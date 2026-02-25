using Azure.Core;
using Performance.Application.Extensions.Repository;
using Performance.Application.DTO;
using Performance.Application.Queries;
using Performance.Domain.Entity;

namespace Performance.Application.Interface.Repository
{
    public interface IUserRepositories : IBaseRepositories<User>
    {
        Task<(IQueryable<User> Items, int TotalCount)> GetPaginatedUsersByOffset(OffsetPaginationRequest request, UserIncludeOptions includeOptions);

        Task<(IQueryable<User> Items, int TotalCount)> GetPaginatedUsersByCursor(CursorPaginationRequest request, UserIncludeOptions includeOptions);
    }
}
