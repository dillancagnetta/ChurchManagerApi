using System.Text.Json;
using ChurchManager.Infrastructure.Abstractions.Configuration;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using CodeBoss.MultiTenant;
using Microsoft.Extensions.Caching.Distributed;

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class QueryCache: IQueryCache
{
    private readonly IDistributedCache _cache;
    private readonly ITenantCurrentUser _currentUser;
    private readonly DistributedCacheEntryOptions _options;

    public QueryCache(IDistributedCache cache, ITenantCurrentUser currentUser, AppConfig config)
    {
        _cache = cache;
        _currentUser = currentUser;
        _options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(config.DefaultCacheTimeMinutes) // Default cache duration
        };
    }
    
    public async Task<T> GetOrSetAsync<T>(string cacheKey, Func<Task<T>> dataRetriever, DistributedCacheEntryOptions options = null,
        CancellationToken ct = default)
    {
        var key = $"{_currentUser?.Tenant}_{cacheKey}";
        var cachedData = await _cache.GetStringAsync(key,  ct);
        
        if (cachedData != null)  return JsonSerializer.Deserialize<T>(cachedData);
        
        var data = await dataRetriever();
        
        if (data != null) await _cache.SetStringAsync(key, JsonSerializer.Serialize(data), options ?? _options, ct);

        return data;
    }

    public Task InvalidateAsync(string cacheKey, CancellationToken ct = default) => _cache.RemoveAsync(cacheKey, ct);
}