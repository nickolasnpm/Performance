namespace Performance.Application.Extensions.Mapping
{
    public static class MapperExtensions
    {
        extension<TSource, TDto>(TSource source) 
            where TSource : class 
            where TDto : class
        {
            public TDto MapEntityToDTO(Func<TSource, TDto> mapper)
                => mapper(source);
        }

        extension<TSource, TDto>(IEnumerable<TSource> source) 
            where TSource : class 
            where TDto : class
        {
            public List<TDto> MapEntityToDTO(Func<TSource, TDto> mapper)
                => source.Select(mapper).ToList();
        }

        extension<TDto, TEntity>(List<TDto> dto) 
            where TDto : class 
            where TEntity : class
        {
            public List<TEntity> MapDtoToEntity(Func<TDto, TEntity> mapper)
                => dto.Select(mapper).ToList();
        }
    }
}