namespace Performance.Infrastructure.Caching
{
    public class CacheItem<T>
    {
        public T? Value { get; set; }
        public CancellationTokenSource? RefreshTokenSource { get; set; }
    }
}
