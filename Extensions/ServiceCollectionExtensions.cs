using Easy.Cache;
using Easy.Cache.Abstractions;
using Easy.Cache.Providers;
using Easy.Cache.Serializers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace Easy.Cache.Extensions
{
    /// <summary>
    /// Extension methods for setting up Easy.Cache services in an IServiceCollection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers MemoryCache as the cache provider.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddEasyCacheMemory(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.TryAddSingleton<ICacheProvider, MemoryCacheProvider>();
            services.TryAddSingleton<CacheManager>();
            return services;
        }

        /// <summary>
        /// Registers Redis as the cache provider.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="redisConnection">The Redis connection string.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddEasyCacheRedis(this IServiceCollection services, string redisConnection)
        {
            services.TryAddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));
            services.TryAddSingleton<ISerializer, JsonCacheSerializer>();
            services.TryAddSingleton<ICacheProvider, RedisCacheProvider>();
            services.TryAddSingleton<CacheManager>();
            return services;
        }

        /// <summary>
        /// Registers a Multi-Layer (Memory + Redis) cache provider.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="redisConnection">The Redis connection string.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddEasyCacheMultiLayer(this IServiceCollection services, string redisConnection)
        {
            services.AddMemoryCache();
            services.TryAddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));
            services.TryAddSingleton<ISerializer, JsonCacheSerializer>();

            // Register concrete implementations so they can be injected into MultiLayerCacheProvider constructor
            services.TryAddSingleton<MemoryCacheProvider>();
            services.TryAddSingleton<RedisCacheProvider>();

            services.TryAddSingleton<ICacheProvider>(sp =>
                new MultiLayerCacheProvider(
                    sp.GetRequiredService<MemoryCacheProvider>(),
                    sp.GetRequiredService<RedisCacheProvider>()));

            services.TryAddSingleton<CacheManager>();

            return services;
        }
    }
}