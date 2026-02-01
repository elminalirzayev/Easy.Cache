using Easy.Cache.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace Easy.Cache.Providers
{
    /// <summary>
    /// Implementation of ICacheProvider using Microsoft.Extensions.Caching.Memory (In-Memory).
    /// </summary>
    public class MemoryCacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCacheProvider"/> class.
        /// </summary>
        /// <param name="memoryCache">The memory cache instance.</param>
        public MemoryCacheProvider(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <inheritdoc />
        public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            // MemoryCache is synchronous, so we wrap it in Task.FromResult
            return Task.FromResult(_memoryCache.TryGetValue(key, out T? value) ? value : default);
        }

        /// <inheritdoc />
        public Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
        {
            var options = new MemoryCacheEntryOptions();

            if (absoluteExpiration.HasValue)
                options.SetAbsoluteExpiration(absoluteExpiration.Value);

            if (slidingExpiration.HasValue)
                options.SetSlidingExpiration(slidingExpiration.Value);

            _memoryCache.Set(key, value, options);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            _memoryCache.Remove(key);
            return Task.CompletedTask;
        }
    }
}