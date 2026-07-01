namespace Performance.Application.Interface.Repository
{
    public interface IBaseRepositories<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        Task Create(IEnumerable<TEntity> entities);
        Task Update(IEnumerable<TEntity> entities);
        Task Delete(HashSet<long> ids);
    }
}