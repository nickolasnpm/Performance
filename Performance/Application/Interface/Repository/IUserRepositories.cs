using Performance.Application.Extensions.Repository.EntityIncludeOptions;
using Performance.Domain.Entity;
using Performance.Application.Common.Models;
using Performance.Application.DTOs;

namespace Performance.Application.Interface.Repository
{
    public interface IUserRepositories : IBaseRepositories<User>
    {
        Task<PaginatedResult<User>> GetPaginatedUsersByOffset(OffsetPaginationRequest request, UserIncludeOptions includeOptions);
        Task<PaginatedResult<User>> GetPaginatedUsersByCursor(long cursorValue, CursorPaginationRequest request, UserIncludeOptions includeOptions);
    }
}
