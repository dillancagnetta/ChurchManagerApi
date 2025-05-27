using ChurchManager.Domain.Common;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace ChurchManager.TenantManager.Data;

public class MasterDbTenantProvider(
    MasterDbContext dbContext,
    IQueryCache cache) : ITenantsProvider<TenantConfiguration>
{
    private DistributedCacheEntryOptions _cacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
    };
    
    public TenantConfiguration[]? Tenants()
    {
        // Check cache first
        var cacheKey = $"TenantConfig_tenants_all";
        var tenants = cache.GetOrSetAsync(cacheKey,
            () =>  dbContext.Tenants.ToListAsync(),
            _cacheOptions
        ).Result;
        
        return tenants?.ToArray();
    }

    public TenantConfiguration Get(string tenantName)
    {
        // Check cache first
        var cacheKey = $"TenantConfig_{tenantName}";
        TenantConfiguration? tenant = cache.GetOrSetAsync(cacheKey,
            () =>  dbContext.Tenants.FirstOrDefaultAsync(tc => tc.Name == tenantName),
            _cacheOptions
        ).Result;
        
        return tenant!;
    }

    public bool Enabled => true;
    public ITenant CurrentTenant { get; set; } 
}