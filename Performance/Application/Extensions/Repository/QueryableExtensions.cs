using Microsoft.EntityFrameworkCore;

namespace Performance.Application.Extensions.Repository
{
    public static class QueryableExtensions
    {
        extension<TEntity>(IQueryable<TEntity> query) where TEntity : class
        {
            public IQueryable<TEntity> ApplyIncludes(IncludeOptions<TEntity> options)
                => options.ApplyTo(query);
        }
    }


    // record has structural equality that compare every property value one by one
    public abstract record IncludeOptions<TEntity> where TEntity : class
    {
        internal abstract IQueryable<TEntity> ApplyTo(IQueryable<TEntity> query);
    }
}