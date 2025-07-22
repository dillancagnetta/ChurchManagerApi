using ChurchManager.Domain.Common;
using ChurchManager.Infrastructure.Abstractions.AppContext;
using ChurchManager.Infrastructure.Abstractions.Configuration;
using ChurchManager.Infrastructure.Abstractions.MultiTenancy;
using CodeBoss.MultiTenant;
using Microsoft.Extensions.Logging;

namespace ChurchManager.TenantManager.Services;

public class TenantUrlResolver(
    ITenantsProvider<TenantConfiguration> tenantProvider,
    AppConfig appConfig,
    IAppContextAccessor contextAccessor,
    ILogger<TenantUrlResolver> logger) : ITenantUrlResolver
{
    public string CurrentTenantHostUrl()
    {
        var tenantName = contextAccessor.AppContext.CurrentTenant.Name;
        return TenantApiUrl(tenantName);
    }

    public string TenantApiUrl(string tenantName)
    {
        var tenant = tenantProvider.Get(tenantName);
        if (tenant == null)
        {
            logger.LogWarning("Tenant {TenantName} not found", tenantName);
            throw new ArgumentException($"Tenant '{tenantName}' not found");
        }
        
        return GetTenantApiUrl(tenant);
    }

    public string WebhookUrl(string tenantName, string webhookPath)
    {
        var tenant = tenantProvider.Get(tenantName);
        if (tenant == null)
        {
            throw new ArgumentException($"Tenant '{tenantName}' not found");
        }
        
        return GetWebhookUrl(tenant, webhookPath);
    }

    public string CurrentTenantWebhookUrl(string webhookPath)
    {
        var tenantName = contextAccessor.AppContext.CurrentTenant.Name;
        return WebhookUrl(tenantName, webhookPath);
    }

    public string CurrentTenantSubDomainUrl(string path)
    {
        string scheme = "https";
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "";
        if (environment.Equals("Development", StringComparison.OrdinalIgnoreCase))
        {
            scheme = "http";
        }
        
        var tenantName = contextAccessor.AppContext.CurrentTenant.Name;
        var tenant = tenantProvider.Get(tenantName);

        var hostUrl = $"{scheme}://{tenant.Subdomain}";
        // Ensure path starts with /
        if (!path.StartsWith("/"))
        {
            path = "/" + path;
        }
        
        var webhookUrl = $"{hostUrl}{path}";
        return webhookUrl;
    }

    private string GetTenantApiUrl(TenantConfiguration tenant)
    {
        var baseDomain = appConfig.BaseDomain;
        
        if (string.IsNullOrEmpty(tenant.ApiUrl))
        {
            logger.LogWarning("Tenant {TenantName} has no subdomain configured", tenant.Name);
            throw new InvalidOperationException($"Tenant '{tenant.Name}' has no subdomain configured");
        }
        
        logger.LogDebug("Found Api URL {HostUrl} for tenant {TenantName}", tenant.ApiUrl, tenant.Name);
        
        return tenant.ApiUrl;
    }

    private string GetWebhookUrl(TenantConfiguration tenant, string webhookPath)
    {
        var hostUrl = GetTenantApiUrl(tenant);
        
        // Ensure webhookPath starts with /
        if (!webhookPath.StartsWith("/"))
        {
            webhookPath = "/" + webhookPath;
        }
        
        var webhookUrl = $"{hostUrl}{webhookPath}";
        
        logger.LogDebug("Generated webhook URL {WebhookUrl} for tenant {TenantName}", webhookUrl, tenant.Name);
        
        return webhookUrl;
    }
}