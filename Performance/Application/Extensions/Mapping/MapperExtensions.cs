namespace Performance.Application.Extensions.Mapping
{
    public static class MapperExtensions
    {
        extension<TSource, TTarget>(TSource source) 
            where TSource : class 
            where TTarget : class
        {
            public TTarget Map(Func<TSource, TTarget> mapper)
                => mapper(source);
        }

        extension<TSource, TTarget>(IEnumerable<TSource> source) 
            where TSource : class 
            where TTarget : class
        {
            public List<TTarget> Map(Func<TSource, TTarget> mapper)
                => source.Select(mapper).ToList();
        }
    }
}