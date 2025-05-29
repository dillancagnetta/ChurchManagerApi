using ChurchManager.Domain.Common;
using ChurchManager.Infrastructure.Abstractions.AppContext;
using CodeBoss.MultiTenant;

namespace ChurchManager.Infrastructure.Shared.AppContext;

public class AppContextSetter(ITenantsProvider<TenantConfiguration> tenantsProvider) : IAppContextSetter
{
    public async Task<IAppContext> InitializeAppContext(string subdomain, string? tenantName = null)
    {
       var tenant = CurrentTenant(tenantName);
       
       var context = new CurrentAppContext(tenant, subdomain);
       
       return await Task.FromResult(context);
    }

    protected ITenant CurrentTenant(string? tenantName = null)
    {
        // Attempt to get tenant from passed in tenant
        var currentTenant = tenantsProvider.Get(tenantName);
        if (currentTenant != null) return currentTenant;
        
        // Fallback: return the first available 
        return tenantsProvider.Tenants().First();
    }
    
    private sealed class CurrentAppContext : IAppContext
    {
        public ITenant CurrentTenant { get; set; }
        public string CurrentSubdomain { get;  set;}

        public CurrentAppContext(ITenant tenant, string subdomain)
        {
            CurrentTenant = tenant;
            CurrentSubdomain = subdomain;
        }
    }
}

