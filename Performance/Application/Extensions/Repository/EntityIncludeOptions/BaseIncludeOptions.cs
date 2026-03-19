namespace Performance.Application.Extensions.Repository.EntityIncludeOptions
{
    // record has structural equality that compare every property value one by one
    public abstract record BaseIncludeOptions<TEntity> where TEntity : class
    {
        internal abstract IQueryable<TEntity> ApplyTo(IQueryable<TEntity> query);
    }
}