using Performance.Application;
using System.Collections.Concurrent;

namespace Performance.Infrastructure.Caching
{
    public static class AsyncCache<T>
    {
        private static readonly ConcurrentDictionary<string, CacheItem<T>> _cache = new();

        public static async Task<T> GetOrUpdateAsync(string key, TimeSpan interval, Func<Task<T>> factory, CancellationToken cancellationToken = default)
        {
            if (_cache.TryGetValue(key, out var item))
            {
                return item.Value!;
            }
            else
            {
                return await CreateCacheAsync(key, interval, factory, cancellationToken);
            }
        }

        private static async Task AutoRefreshAsync(string key, TimeSpan interval, Func<Task<T>> factory, CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(interval, cancellationToken);

                    if (cancellationToken.IsCancellationRequested)
                        break;

                    var newValue = await factory();

                    if (_cache.TryGetValue(key, out var item))
                    {
                        item.Value = newValue;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Expected when cancellation is requested
            }
            catch (Exception)
            {
                // Log error or handle refresh failures
                // Continue running to retry on next interval
            }
        }

        private static async Task<T> CreateCacheAsync(string key, TimeSpan interval, Func<Task<T>> factory, CancellationToken cancellationToken)
        {
            var value = await factory();
            var cts = new CancellationTokenSource();
            var item = new CacheItem<T>
            {
                Value = value,
                RefreshTokenSource = cts
            };

            _cache[key] = item;

            // Start auto-refresh in the background (fire and forget)
            _ = Task.Run(() => AutoRefreshAsync(key, interval, factory, cts.Token), cancellationToken);

            return value;
        }

        public static void Invalidate(string key)
        {
            if (_cache.TryRemove(key, out var item))
            {
                // Cancel the auto-refresh task
                item.RefreshTokenSource?.Cancel();
                item.RefreshTokenSource?.Dispose();
            }
        }

        public static void InvalidateAll()
        {
            foreach (var key in _cache.Keys)
            {
                Invalidate(key);
            }
        }
    }
}