using Performance.Application.DTO;
using Performance.Application.Queries;
using Performance.Domain.Entity;
using Performance.Application.Common.Models;

namespace Performance.Application.Interface.Repository
{
    public interface IUserRepositories : IBaseRepositories<User>
    {
        Task<UserPaginatedResult<User>> GetPaginatedUsersByOffset(OffsetPaginationRequest request, UserIncludeOptions includeOptions);
        Task<UserPaginatedResult<User>> GetPaginatedUsersByCursor(CursorPaginationRequest request, UserIncludeOptions includeOptions);
    }
}
