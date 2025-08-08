namespace Easy.Cache.Abstractions
{
    public interface ICacheProvider
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null);
        Task RemoveAsync(string key);
    }
}
