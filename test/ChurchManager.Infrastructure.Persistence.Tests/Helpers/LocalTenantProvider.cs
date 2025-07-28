using ChurchManager.Domain.Common;
using CodeBoss.MultiTenant;

namespace ChurchManager.Infrastructure.Persistence.Tests.Helpers;

public class LocalTenantProvider : ITenantsProvider<TenantConfiguration>
{
    public bool Enabled => true;
    public TenantConfiguration[] Tenants()
    {
        return new[]
        {
            new TenantConfiguration
            {
                Name = "Tenant1",
                Email = "tenant1@example.com",
                ApiUrl = "https://localhost:5001",
                Subdomain = "tenant1",
                ConnectionString =
                    "Server=localhost;Port=5432;Database=churchmanager_db;User Id=admin;password=P455word1;"
            }
        };
    }

    public TenantConfiguration Get(string name) => Tenants().First();

    public ITenant CurrentTenant
    {
        get => Get("Tenant1");
        set => throw new NotImplementedException();
    }

    ITenant[] ISimpleTenantsProvider.Tenants()
    {
        return Tenants();
    }
}

public class NoneTenantProvider: ITenantsProvider<TenantConfiguration>
{
    public TenantConfiguration[] Tenants()
    {
        throw new NotImplementedException();
    }

    public TenantConfiguration Get(string name)
    {
        throw new NotImplementedException();
    }

    public bool Enabled { get; } = false;
    public ITenant CurrentTenant { get; set; }
    ITenant[] ISimpleTenantsProvider.Tenants()
    {
        return Tenants();
    }
}