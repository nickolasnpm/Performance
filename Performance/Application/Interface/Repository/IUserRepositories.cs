using Performance.Application.DTOs;
using Performance.Application.Extensions.Repository;
using Performance.Domain.Entity;
using Performance.Application.Common.Models;

namespace Performance.Application.Interface.Repository
{
    public interface IUserRepositories : IBaseRepositories<User>
    {
        Task<PaginatedResult<User>> GetPaginatedUsersByOffset(OffsetPaginationRequest request, UserIncludeOptions includeOptions);
        Task<PaginatedResult<User>> GetPaginatedUsersByCursor(CursorPaginationRequest request, UserIncludeOptions includeOptions);
    }
}
