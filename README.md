[![Build & Test](https://github.com/elminalirzayev/Easy.Cache/actions/workflows/build.yml/badge.svg)](https://github.com/elminalirzayev/Easy.Cache/actions/workflows/build.yml)
[![Build & Release](https://github.com/elminalirzayev/Easy.Cache/actions/workflows/release.yml/badge.svg)](https://github.com/elminalirzayev/Easy.Cache/actions/workflows/release.yml)
[![Build & Nuget Publish](https://github.com/elminalirzayev/Easy.Cache/actions/workflows/nuget.yml/badge.svg)](https://github.com/elminalirzayev/Easy.Cache/actions/workflows/nuget.yml)
[![Release](https://img.shields.io/github/v/release/elminalirzayev/Easy.Cache)](https://github.com/elminalirzayev/Easy.Cache/releases)
[![License](https://img.shields.io/github/license/elminalirzayev/Easy.Cache)](https://github.com/elminalirzayev/Easy.Cache/blob/master/LICENSE.txt)
[![NuGet](https://img.shields.io/nuget/v/Easy.Cache.svg)](https://www.nuget.org/packages/Easy.Cache)


# Easy.Cache

Easy.Cache is a lightweight, flexible caching library for .NET applications supporting multi-layer caching with MemoryCache and Redis. It provides seamless serialization and optional expiration policies.

## Features

- Multi-layer cache with MemoryCache and Redis
- Supports serialization with customizable serializers
- Optional expiration support (absolute and sliding expiration for MemoryCache; absolute expiration for Redis)
- Simple API with async support
- Easily extensible and configurable

## Installation

```bash
dotnet add package Easy.Cache
```

## Usage

### MemoryCache
```csharp
builder.Services.AddEasyCacheMemory();
```

### Redis
```csharp
builder.Services.AddEasyCacheRedis("your-redis-connection-string");
```

### Multi-layer (Memory + Redis)
```csharp
builder.Services.AddEasyCacheMultiLayer("your-redis-connection-string");
```

## Example
```csharp
public class TestController : ControllerBase
{
    private readonly CacheManager _cache;

    public TestController(CacheManager cache)
    {
        _cache = cache;
    }

    [HttpGet("set")]
    public async Task<IActionResult> SetData()
    {
        TimeSpan absoluteExpiration = TimeSpan.FromMinutes(10);
        TimeSpan slidingExpiration = TimeSpan.FromMinutes(2);

        await _cache.SetAsync("user:1", new { Name = "John", Age = 30 }, absoluteExpiration, slidingExpiration);
        //or
        //for Redis only:
        //await _cache.SetAsync("user:1", new { Name = "John", Age = 30 }, absoluteExpiration);
        //or
        //await _cache.SetAsync("user:1", new { Name = "John", Age = 30 }); // No expiration
        return Ok();
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetData()
    {
        var user = await _cache.GetAsync<dynamic>("user:1");
        return Ok(user);
    }
}
```

### Notes on Expiration
-  MemoryCache supports both absolute and sliding expiration.
-  Redis supports only absolute expiration (TTL). Sliding expiration is not natively supported on Redis.
-  If no expiration is provided, cached data will remain indefinitely until removed.


## Serializer
-  Easy.Cache uses an ISerializer interface to support flexible serialization. A built-in JSON serializer (JsonCacheSerializer) is provided.

## Contributing
- Contributions are welcome. Please open issues or pull requests for improvements or bug fixes.


## License
MIT License

---

© 2025 Elmin Alirzayev / Easy Code Tools
