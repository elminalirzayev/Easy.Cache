using Easy.Cache.Abstractions;

namespace Easy.Cache
{
    /// <summary>
    /// Manager class that acts as a facade for the underlying ICacheProvider.
    /// Simplifies cache operations for the end user.
    /// </summary>
    public class CacheManager
    {
        private readonly ICacheProvider _cacheProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheManager"/> class.
        /// </summary>
        /// <param name="cacheProvider">The configured cache provider.</param>
        public CacheManager(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        /// <summary>
        /// Sets an item in the cache.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="key">Unique cache key.</param>
        /// <param name="value">The value to store.</param>
        /// <param name="absoluteExpiration">Absolute expiration timespan.</param>
        /// <param name="slidingExpiration">Sliding expiration timespan.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
        {
            return _cacheProvider.SetAsync(key, value, absoluteExpiration, slidingExpiration, cancellationToken);
        }

        /// <summary>
        /// Retrieves an item from the cache.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="key">Unique cache key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The cached value or default.</returns>
        public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            return _cacheProvider.GetAsync<T>(key, cancellationToken);
        }

        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        /// <param name="key">Unique cache key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            return _cacheProvider.RemoveAsync(key, cancellationToken);
        }
    }
}