using Easy.Cache.Abstractions;
using Easy.Cache.Providers;
using Easy.Cache.Serializers;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Text.Json;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEasyCacheMemory(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<ICacheProvider, MemoryCacheProvider>();
        services.AddSingleton<CacheManager>();
        return services;
    }

    public static IServiceCollection AddEasyCacheRedis(this IServiceCollection services, string redisConnection)
    {
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));
        services.AddSingleton<ISerializer, JsonCacheSerializer>();
        services.AddSingleton<ICacheProvider, RedisCacheProvider>();
        services.AddSingleton<CacheManager>();
        return services;
    }

    public static IServiceCollection AddEasyCacheMultiLayer(this IServiceCollection services, string redisConnection)
    {
        services.AddMemoryCache();
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));
        services.AddSingleton<ISerializer, JsonCacheSerializer>();

        services.AddSingleton<MemoryCacheProvider>();
        services.AddSingleton<RedisCacheProvider>();

        services.AddSingleton<ICacheProvider>(sp =>
            new MultiLayerCacheProvider(
                sp.GetRequiredService<MemoryCacheProvider>(),
                sp.GetRequiredService<RedisCacheProvider>()));

        services.AddSingleton<CacheManager>();

        return services;
    }
}
