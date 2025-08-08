using Easy.Cache.Abstractions;
using StackExchange.Redis;

public class RedisCacheProvider : ICacheProvider
{
    private readonly IDatabase _db;
    private readonly ISerializer _serializer;

    public RedisCacheProvider(IConnectionMultiplexer redis, ISerializer serializer)
    {
        _db = redis.GetDatabase();
        _serializer = serializer;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
    {
        var serialized = _serializer.Serialize(value);

        // Redis sliding expiration doğrudan desteklemez, o yüzden sadece absoluteExpiration kullanıyoruz.
        TimeSpan? expiry = absoluteExpiration ?? slidingExpiration;

        await _db.StringSetAsync(key, serialized, expiry);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var data = await _db.StringGetAsync(key);
        if (data.IsNullOrEmpty)
            return default;

        return _serializer.Deserialize<T>(data);
    }

    public async Task RemoveAsync(string key)
    {
        await _db.KeyDeleteAsync(key);
    }
}
