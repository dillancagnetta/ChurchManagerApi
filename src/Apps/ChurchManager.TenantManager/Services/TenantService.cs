using ChurchManager.Domain.Common;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.TenantManager.Data;
using CodeBoss.MultiTenant;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ChurchManager.TenantManager.Services;

public class TenantService(
    IHttpContextAccessor httpContextAccessor,
    MasterDbContext dbContext,
    IConfiguration configuration,
    IQueryCache cache,
    ITenantCurrentUser tenantCurrentUser
    ): ITenantService
{
    public string CurrentTenantId()
    {
        return tenantCurrentUser.Tenant;
    }

    public Task<TenantConfiguration?>? TenantConfigurationAsync()
    {
        var tenantName = CurrentTenantId();
        return TenantConfigurationAsync(tenantName);
    }

    public async Task<TenantConfiguration?>? TenantConfigurationAsync(string tenantName)
    {
        if (string.IsNullOrEmpty(tenantName)) return null;
        
        // Check cache first
        var cacheKey = $"TenantConfig_{tenantName}";
        TenantConfiguration? tenant = await cache.GetOrSetAsync(cacheKey,
             () =>  dbContext.Tenants.FirstOrDefaultAsync(tc => tc.Name == tenantName));
        
        return tenant;
    }
}