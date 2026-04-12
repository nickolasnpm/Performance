namespace Performance.Application.Extensions.Mapping
{
    public static class MapperExtensions
    {
        extension<TSource, TTarget>(TSource source) 
            where TSource : class 
            where TTarget : class
        {
            // get single
            public TTarget MapEntityToDTO(Func<TSource, TTarget> mapper)
                => mapper(source);
        }

        extension<TSource, TTarget>(IEnumerable<TSource> source) 
            where TSource : class 
            where TTarget : class
        {
            // get list
            public List<TTarget> MapEntityToDTO(Func<TSource, TTarget> mapper)
                => source.Select(mapper).ToList();

            // create bulk
            public List<TTarget> MapDTOToEntity(Func<TSource, TTarget> mapper)
                => source.Select(mapper).ToList();
        }

        extension<TSource, TExisting>(List<TSource> dto)
            where TSource : class
            where TExisting : class
        {
            // update bulk
            public void MapDTOToEntity(Func<TSource, TExisting> keySelector, Action<TSource, TExisting> mapper)
                => dto.ForEach(d => mapper(d, keySelector(d)));
        }
    }
}