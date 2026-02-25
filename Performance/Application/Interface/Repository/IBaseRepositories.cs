using Performance.Application.DTO;
using Performance.Application.Queries;
using Performance.Domain.Entity;

namespace Performance.Application.Interface.Repository
{
    public interface IBaseRepositories<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
    }
}

