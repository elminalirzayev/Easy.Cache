namespace Easy.Cache.Abstractions
{
    /// <summary>
    /// Defines the contract for a cache provider mechanism.
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Retrieves an item from the cache asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="key">The unique cache key.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>The cached item, or default if not found.</returns>
        Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets an item in the cache asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="key">The unique cache key.</param>
        /// <param name="value">The value to cache.</param>
        /// <param name="absoluteExpiration">The absolute expiration time relative to now.</param>
        /// <param name="slidingExpiration">The sliding expiration time window.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an item from the cache asynchronously.
        /// </summary>
        /// <param name="key">The unique cache key.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    }
}