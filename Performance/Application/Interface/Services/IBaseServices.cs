using Performance.Application.Common.Models;

namespace Performance.Application.Interface.Services
{
    public interface IBaseServices<TResponse, TRequest> 
        where TResponse : class
        where TRequest : class
    {
        Task<Result<ListResponseDTO<TResponse>, ResultError>> GetPaginatedListAsync(TRequest request);
        Task<Result<TResponse, ResultError>> GetByIdAsync(string hashId);
    }
}
