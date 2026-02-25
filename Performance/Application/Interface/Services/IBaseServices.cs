namespace Performance.Application.Interface.Services
{
    public interface IBaseServices<TEntity, TResponse, TRequest> 
        where TEntity : class
        where TResponse : class
        where TRequest : class
    {
        IQueryable<TEntity> GetAllAsync();
        Task<TResponse> GetPaginatedListAsync(TRequest request);
    }
}
