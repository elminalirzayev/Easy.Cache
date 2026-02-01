using Easy.Cache.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Cache.Providers
{
    /// <summary>
    /// A composite cache provider that utilizes both Memory (L1) and Redis (L2) caching layers.
    /// </summary>
    public class MultiLayerCacheProvider : ICacheProvider
    {
        private readonly MemoryCacheProvider _memoryProvider;
        private readonly RedisCacheProvider _redisProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLayerCacheProvider"/> class.
        /// </summary>
        /// <param name="memoryProvider">The Level 1 (Memory) cache provider.</param>
        /// <param name="redisProvider">The Level 2 (Distributed/Redis) cache provider.</param>
        public MultiLayerCacheProvider(MemoryCacheProvider memoryProvider, RedisCacheProvider redisProvider)
        {
            _memoryProvider = memoryProvider;
            _redisProvider = redisProvider;
        }

        /// <inheritdoc />
        public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
        {
            // Write to both caches
            await _memoryProvider.SetAsync(key, value, absoluteExpiration, slidingExpiration, cancellationToken).ConfigureAwait(false);
            await _redisProvider.SetAsync(key, value, absoluteExpiration, slidingExpiration, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            // 1. Try L1 (Memory)
            var value = await _memoryProvider.GetAsync<T>(key, cancellationToken).ConfigureAwait(false);

            // FIX: Added '!' (null-forgiving operator) to value and default.
            // EqualityComparer handles nulls safely, so we tell the compiler "Trust me, this is fine".
            if (!EqualityComparer<T>.Default.Equals(value!, default!))
            {
                return value;
            }

            // 2. Try L2 (Redis)
            value = await _redisProvider.GetAsync<T>(key, cancellationToken).ConfigureAwait(false);

            // FIX: Added '!' here as well.
            if (!EqualityComparer<T>.Default.Equals(value!, default!))
            {
                // If found in Redis, populate Memory Cache (L1) for future fast access
                await _memoryProvider.SetAsync(key, value!, cancellationToken: cancellationToken).ConfigureAwait(false);
            }

            return value;
        }

        /// <inheritdoc />
        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _memoryProvider.RemoveAsync(key, cancellationToken).ConfigureAwait(false);
            await _redisProvider.RemoveAsync(key, cancellationToken).ConfigureAwait(false);
        }
    }
}