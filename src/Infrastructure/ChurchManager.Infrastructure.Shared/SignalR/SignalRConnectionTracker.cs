using System.Text.Json;
using ChurchManager.Infrastructure.Abstractions.SignalR;
using Microsoft.Extensions.Caching.Distributed;

namespace ChurchManager.Infrastructure.Shared.SignalR;

public class SignalRConnectionTracker(IDistributedCache cache) : IConnectionTracker
{
    private const string KeyPrefix = "user-connections:";
    private readonly TimeSpan _expirationTime = TimeSpan.FromHours(24);

    public async Task AddConnectionAsync(string userId, string connectionId)
    {
        var key = $"{KeyPrefix}{userId}";
        var connections = await GetConnectionsAsync(userId);
        connections.Add(connectionId);
        await cache.SetAsync(key, 
            JsonSerializer.SerializeToUtf8Bytes(connections),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _expirationTime
            });
    }

    public async Task RemoveConnectionAsync(string userId, string connectionId)
    {
        var key = $"{KeyPrefix}{userId}";
        var connections = await GetConnectionsAsync(userId);
        connections.Remove(connectionId);
        
        if (connections.Count > 0)
        {
            await cache.SetAsync(key, 
                JsonSerializer.SerializeToUtf8Bytes(connections),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _expirationTime
                });
        }
        else
        {
            await cache.RemoveAsync(key);
        }
    }

    public async Task<bool> IsUserConnectedAsync(string userId)
    {
        var connections = await GetConnectionsAsync(userId);
        return connections.Count > 0;
    }

    private async Task<HashSet<string>> GetConnectionsAsync(string userId)
    {
        var key = $"{KeyPrefix}{userId}";
        var data = await cache.GetAsync(key);
        if (data != null)
        {
            return JsonSerializer.Deserialize<HashSet<string>>(data) ?? new HashSet<string>();
        }
        return new HashSet<string>();
    }
}