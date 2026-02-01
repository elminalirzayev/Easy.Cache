[![Build & Test](https://github.com/elminalirzayev/Easy.Cache/actions/workflows/build.yml/badge.svg)](https://github.com/elminalirzayev/Easy.Cache/actions/workflows/build.yml)
[![Build & Release](https://github.com/elminalirzayev/Easy.Cache/actions/workflows/release.yml/badge.svg)](https://github.com/elminalirzayev/Easy.Cache/actions/workflows/release.yml)
[![Build & Nuget Publish](https://github.com/elminalirzayev/Easy.Cache/actions/workflows/nuget.yml/badge.svg)](https://github.com/elminalirzayev/Easy.Cache/actions/workflows/nuget.yml)
[![Release](https://img.shields.io/github/v/release/elminalirzayev/Easy.Cache)](https://github.com/elminalirzayev/Easy.Cache/releases)
[![License](https://img.shields.io/github/license/elminalirzayev/Easy.Cache)](https://github.com/elminalirzayev/Easy.Cache/blob/master/LICENSE.txt)
[![NuGet](https://img.shields.io/nuget/v/Easy.Cache.svg)](https://www.nuget.org/packages/Easy.Cache)


# Easy.Cache

**Easy.Cache** is a high-performance, lightweight, and flexible caching library for .NET applications. It simplifies cache management by providing a unified API for **In-Memory**, **Distributed (Redis)**, and **Multi-Layer (Hybrid)** caching strategies.

It is designed with **Enterprise Best Practices** in mind, offering full `async/await` support with `CancellationToken`, strong typing, and modular architecture.


##  Features

- ** Multi-Layer Caching:** Automatically syncs Local Memory Cache (L1) with Distributed Redis Cache (L2) for ultra-fast reads and data consistency.
- ** Async & Cancellable:** Fully supports `async/await` and `CancellationToken` to prevent wasted resources on cancelled requests.
- ** Plug & Play:** Simple integration with .NET Dependency Injection (`IServiceCollection`).
- ** Modular Design:** Separated into `Abstractions`, `Providers`, and `Serializers` namespaces for cleaner architecture.
- ** Multi-Target:** Supports `.NET 10`, `.NET 8`, `.NET 6`, `.NET Standard 2.0/2.1`, and `.NET Framework 4.7.2+`.


##  Installation

Install via NuGet Package Manager:

```bash
Install-Package Easy.Cache
```

Or via .NET CLI:

```bash
dotnet add package Easy.Cache
```


##  Usage

### 1. Service Registration (Program.cs)

Register the caching strategy you need in your `Program.cs`.


```csharp
using Easy.Cache; // Core Namespace

var builder = WebApplication.CreateBuilder(args);

// Option A: In-Memory Cache (Single Server)
builder.Services.AddEasyCacheMemory();

// Option B: Redis Cache (Distributed)
builder.Services.AddEasyCacheRedis("localhost:6379,password=...");

// Option C: Multi-Layer Cache (Best of Both Worlds)
// Reads from Memory first (fast), falls back to Redis (reliable).
builder.Services.AddEasyCacheMultiLayer("localhost:6379,password=...");

var app = builder.Build();
```

### 2. Using CacheManager (Controller Example)

Inject `CacheManager` into your controllers or services.

```csharp
using Easy.Cache;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly CacheManager _cache;

    public ProductsController(CacheManager cache)
    {
        _cache = cache;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id, CancellationToken cancellationToken)
    {
        string cacheKey = $"product:{id}";

        // 1. Try to get from cache
        var product = await _cache.GetAsync<Product>(cacheKey, cancellationToken);

        if (product != null)
        {
            return Ok(product); // Hit!
        }

        // 2. If not found, fetch from DB (Simulated)
        product = new Product { Id = id, Name = "Smartphone", Price = 999 };

        // 3. Set to cache
        // Absolute Expiration: 10 minutes (Removed after 10m)
        // Sliding Expiration: 2 minutes (Reset timer if accessed within 2m)
        await _cache.SetAsync(
            cacheKey, 
            product, 
            absoluteExpiration: TimeSpan.FromMinutes(10), 
            slidingExpiration: TimeSpan.FromMinutes(2), 
            cancellationToken: cancellationToken
        );

        return Ok(product);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveProduct(int id, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync($"product:{id}", cancellationToken);
        return NoContent();
    }
}
```


##  Expiration Policies

| Provider | Absolute Expiration | Sliding Expiration | Note |
| --- | --- | --- | --- |
| **MemoryCache** | ✅ Supported | ✅ Supported | Full support for both strategies. |
| **Redis** | ✅ Supported | ❌ Not Supported | Redis native "TTL" is used as Absolute Expiration. |

Export to Sheets

> **Note:** If `absoluteExpiration` and `slidingExpiration` are both null, the cache entry will remain indefinitely until manually removed or evicted by memory pressure.


## Architecture

The library follows a clean separation of concerns:

-   **`Easy.Cache.Abstractions`**: Interfaces (`ICacheProvider`, `ISerializer`). Use this namespace if you are building libraries that depend on Easy.Cache.
    
-   **`Easy.Cache.Providers`**: Concrete implementations (`RedisCacheProvider`, `MemoryCacheProvider`, `MultiLayerCacheProvider`).
    
-   **`Easy.Cache.Serializers`**: Serialization logic (`JsonCacheSerializer`).
    
-   **`Easy.Cache`**: The main entry point (`CacheManager`, `ServiceCollectionExtensions`).
    

---

## Contributing

Contributions and suggestions are welcome. Please open an issue or submit a pull request.

---

## Contact

For questions, contact us via elmin.alirzayev@gmail.com or GitHub.

---

## License

This project is licensed under the MIT License.

---

© 2025 Elmin Alirzayev / Easy Code Tools