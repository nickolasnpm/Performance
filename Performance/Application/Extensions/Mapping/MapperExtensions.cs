namespace Performance.Application.Extensions.Mapping
{
    public static class MapperExtensions
    {
        extension<TSource, TDto>(TSource source) where TSource : class
        {
            public TDto MapEntityToDTO(Func<TSource, TDto> mapper)
                => mapper(source);
        }

        extension<TSource, TDto>(IEnumerable<TSource> source)
        {
            public List<TDto> MapEntityToDTO(Func<TSource, TDto> mapper)
                => source.Select(mapper).ToList();
        }
    }
}