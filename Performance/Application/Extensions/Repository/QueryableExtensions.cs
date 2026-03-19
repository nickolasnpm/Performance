using Performance.Application.Extensions.Repository.EntityIncludeOptions;

namespace Performance.Application.Extensions.Repository
{
    public static class QueryableExtensions
    {
        extension<TEntity>(IQueryable<TEntity> query) where TEntity : class
        {
            public IQueryable<TEntity> ApplyIncludes(BaseIncludeOptions<TEntity> options)
                => options.ApplyTo(query);
        }
    }
}