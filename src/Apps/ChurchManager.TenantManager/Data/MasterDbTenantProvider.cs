using ChurchManager.Domain.Common;
using ChurchManager.Infrastructure.Abstractions.AppContext;
using ChurchManager.Infrastructure.Abstractions.Configuration;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace ChurchManager.TenantManager.Data;

public class MasterDbTenantProvider(
    MasterDbContext dbContext,
    IAppContextAccessor contextAccessor,
    IEnvironmentConfig envConfig,
    IQueryCache cache,
    ILogger<MasterDbTenantProvider> logger) : ITenantsProvider<TenantConfiguration>
{
    private readonly DistributedCacheEntryOptions _cacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
    };
    
    /// <summary>
    /// CurrentSubdomain is now set from the EnvironmentConfig,
    /// So this will migrate all tenants of the current subdomain
    /// </summary>
    public TenantConfiguration[]? Tenants()
    {
        var subdomain = CurrentSubdomain;
        var query = dbContext.Tenants.AsQueryable().AsNoTracking();
        if (subdomain is not null)  query = query.Where(x => x.Subdomain == subdomain);
        
        // Check cache first
        var cacheKey = $"TenantConfig_tenants_all_{subdomain}";
        var tenants = cache.GetOrSetAsync(cacheKey,
            () =>  query.ToListAsync(),
            _cacheOptions
        ).Result;
        
        logger.LogInformation("Retrieved {TenantCount} tenants for subdomain: [{Subdomain}] with cache key: [{CacheKey}]", 
            tenants?.Count ?? 0, 
            subdomain ?? "", 
            cacheKey);
        
        return tenants?.ToArray();
    }

    public TenantConfiguration? Get(string? tenantName)
    {
        tenantName = tenantName?.Trim()?.ToLowerInvariant();
        // Check cache first
        var cacheKey = $"TenantConfig_{tenantName}";
        TenantConfiguration? tenant = cache.GetOrSetAsync(cacheKey,
            () =>  dbContext.Tenants.AsQueryable().AsNoTracking()
                .FirstOrDefaultAsync(tc => tc.Name == tenantName),
            _cacheOptions
        ).Result;
        
        logger.LogInformation("Retrieved tenant: {tenantName} with cache key {CacheKey}", 
            tenantName, 
            cacheKey);
        
        return tenant;
    }

    private string? CurrentSubdomain => contextAccessor.AppContext?.CurrentSubdomain ?? envConfig.SubdomainKey;
 
    public bool Enabled => true;
    ITenant[] ISimpleTenantsProvider.Tenants()
    {
        return Tenants();
    }
}