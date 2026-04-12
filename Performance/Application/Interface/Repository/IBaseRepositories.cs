namespace Performance.Application.Interface.Repository
{
    public interface IBaseRepositories<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        Task<List<TEntity>> Create(List<TEntity> entities);
        Task<bool> Delete(HashSet<long> ids);
    }
}