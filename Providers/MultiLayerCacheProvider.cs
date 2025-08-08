using Easy.Cache.Abstractions;

public class MultiLayerCacheProvider : ICacheProvider
{
    private readonly ICacheProvider _memoryProvider;
    private readonly ICacheProvider _redisProvider;

    public MultiLayerCacheProvider(ICacheProvider memoryProvider, ICacheProvider redisProvider)
    {
        _memoryProvider = memoryProvider;
        _redisProvider = redisProvider;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
    {
        await _memoryProvider.SetAsync(key, value, absoluteExpiration, slidingExpiration);
        await _redisProvider.SetAsync(key, value, absoluteExpiration, slidingExpiration);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _memoryProvider.GetAsync<T>(key);
        if (value != null && !value.Equals(default(T)))
            return value;

        value = await _redisProvider.GetAsync<T>(key);
        if (value != null && !value.Equals(default(T)))
            await _memoryProvider.SetAsync(key, value);

        return value;
    }

    public async Task RemoveAsync(string key)
    {
        await _memoryProvider.RemoveAsync(key);
        await _redisProvider.RemoveAsync(key);
    }
}
