using Easy.Cache.Abstractions;

public class CacheManager
{
    private readonly ICacheProvider _cacheProvider;

    public CacheManager(ICacheProvider cacheProvider)
    {
        _cacheProvider = cacheProvider;
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
    {
        return _cacheProvider.SetAsync(key, value, absoluteExpiration, slidingExpiration);
    }

    public Task<T?> GetAsync<T>(string key)
    {
        return _cacheProvider.GetAsync<T>(key);
    }

    public Task RemoveAsync(string key)
    {
        return _cacheProvider.RemoveAsync(key);
    }
}
