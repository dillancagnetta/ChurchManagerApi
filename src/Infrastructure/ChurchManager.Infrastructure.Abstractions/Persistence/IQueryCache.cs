using Microsoft.Extensions.Caching.Distributed;

namespace ChurchManager.Infrastructure.Abstractions.Persistence;

public interface IQueryCache
{
    Task<T> GetOrSetAsync<T>(string cacheKey, Func<Task<T>> dataRetriever, DistributedCacheEntryOptions options = null, CancellationToken ct = default);
    Task InvalidateAsync(string cacheKey, CancellationToken ct = default);
}