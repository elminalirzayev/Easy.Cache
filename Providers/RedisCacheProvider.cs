using Easy.Cache.Abstractions;
using StackExchange.Redis;

namespace Easy.Cache.Providers
{
    /// <summary>
    /// Implementation of ICacheProvider using StackExchange.Redis.
    /// </summary>
    public class RedisCacheProvider : ICacheProvider
    {
        private readonly IDatabase _db;
        private readonly ISerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheProvider"/> class.
        /// </summary>
        /// <param name="redis">The Redis connection multiplexer.</param>
        /// <param name="serializer">The serializer for object conversion.</param>
        public RedisCacheProvider(IConnectionMultiplexer redis, ISerializer serializer)
        {
            _db = redis.GetDatabase();
            _serializer = serializer;
        }

        /// <inheritdoc />
        public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
        {
            // Redis does not natively support Sliding Expiration in the same way MemoryCache does.
            // We typically use Absolute Expiration (TTL) for Redis.
            TimeSpan? expiry = absoluteExpiration ?? slidingExpiration;

            var serialized = _serializer.Serialize(value);

            // Execute async operation without blocking

            if (expiry.HasValue)
            {
                await _db.StringSetAsync(key, serialized, (Expiration)expiry).ConfigureAwait(false);

            }
            else
            {
                await _db.StringSetAsync(key, serialized).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var data = await _db.StringGetAsync(key).ConfigureAwait(false);

            if (data.IsNullOrEmpty)
                return default;

            return _serializer.Deserialize<T>(data!);
        }

        /// <inheritdoc />
        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _db.KeyDeleteAsync(key).ConfigureAwait(false);
        }
    }
}