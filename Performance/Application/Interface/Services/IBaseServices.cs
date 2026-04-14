using Performance.Application.Common.Models;
using Performance.Application.DTOs.Users;

namespace Performance.Application.Interface.Services
{
    public interface IBaseServices<TResponse, TRequest> 
        where TResponse : class
        where TRequest : class
    {
        Task<Result<ListResponseDTO<TResponse>, ResultError>> GetPaginatedListAsync(TRequest request);
        Task<Result<TResponse, ResultError>> GetByIdAsync(long Id);
    }
}
