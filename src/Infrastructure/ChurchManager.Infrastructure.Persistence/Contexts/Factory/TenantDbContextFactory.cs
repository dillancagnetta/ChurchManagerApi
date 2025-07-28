using ChurchManager.Domain.Common;
using ChurchManager.Infrastructure.Abstractions.MultiTenancy;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using CodeBoss.MultiTenant;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Infrastructure.Persistence.Contexts.Factory;

public class TenantDbContextFactory(
    IServiceProvider serviceProvider,
    ILogger<TenantDbContextFactory> logger) : ITenantDbContextFactory
{
    public IChurchManagerDbContext CreateDbContext(int? tenantId) => Create(tenantId);
   
    
    private IChurchManagerDbContext Create(int? tenantId)
    {
        try
        {
            if (tenantId.HasValue)
            {
                // Get tenant connection string
                var scope = serviceProvider.CreateScope();
                var tenantsProvider = scope.ServiceProvider.GetRequiredService<ITenantsProvider<TenantConfiguration>>();
                try
                {
                    var tenant = tenantsProvider.Tenants().FirstOrDefault(x => x.Id == tenantId);

                    if (tenant != null)
                    {
                        return DbContextFactory.Create(tenant.ConnectionString, tenantsProvider);
                    }
                }
                finally
                {
                    scope.Dispose();
                }
            }

            // Default/single-tenant connection - create a new scope for this context
            var defaultScope = serviceProvider.CreateScope();
            return defaultScope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating database context for tenant {TenantId}", tenantId);
            throw;
        }
    }
}