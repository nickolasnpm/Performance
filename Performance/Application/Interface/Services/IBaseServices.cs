using Performance.Application.Common.Models;
using Performance.Application.DTOs;

namespace Performance.Application.Interface.Services
{
    public interface IBaseServices<TResponse, TRequest> 
    {
        Task<Result<ListResponseDTO<TResponse>, ResultError>> GetPaginatedListAsync(TRequest request);
        Task<Result<TResponse, ResultError>> GetByIdAsync(string hashId);
    }
}
