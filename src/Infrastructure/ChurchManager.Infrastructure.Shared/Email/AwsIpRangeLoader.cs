using System.Net.Http;
using System.Text.Json;
using ChurchManager.Infrastructure.Abstractions.Network;
using CodeBoss.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Infrastructure.Shared.Email;

public class AwsIpRangeLoader : IAwsIpRangeLoader
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<AwsIpRangeLoader> _logger;
    private readonly HttpClient _httpClient;

    public AwsIpRangeLoader(IDistributedCache cache, ILogger<AwsIpRangeLoader> logger, HttpClient httpClient)
    {
        _cache = cache;
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<HashSet<string>> ReadAllowedIpRanges(string[] regions)
    {
        const string CacheKey = "AwsIpRanges";
        var cachedRanges = await _cache.GetStringAsync(CacheKey);

        if (cachedRanges.IsNullOrEmpty())
        {
            var ipRanges = await FetchAwsIpRanges(regions);
            var serializedRanges = JsonSerializer.Serialize(ipRanges);
            var cacheEntryOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1));
            await _cache.SetStringAsync(CacheKey, serializedRanges, cacheEntryOptions);
            return ipRanges;
        }

        return JsonSerializer.Deserialize<HashSet<string>>(cachedRanges);
    }

    private async Task<HashSet<string>> FetchAwsIpRanges(string[] regions)
    {
        try
        {
            var response = await _httpClient.GetStringAsync("https://ip-ranges.amazonaws.com/ip-ranges.json");
            var document = JsonDocument.Parse(response);
            var prefixes = document.RootElement.GetProperty("prefixes");

            var ipRanges = new HashSet<string>{"127.0.0.1/32"}; // Add localhost IP address
            foreach (var prefix in prefixes.EnumerateArray())
            {
                var service = prefix.GetProperty("service").GetString();
                var region = prefix.GetProperty("region").GetString();
                var ipPrefix = prefix.GetProperty("ip_prefix").GetString();

                if (service == "AMAZON" && regions.Contains(region))
                {
                    ipRanges.Add(ipPrefix);
                }
            }

            return ipRanges;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch AWS IP ranges");
            return new HashSet<string>();
        }
    }
}