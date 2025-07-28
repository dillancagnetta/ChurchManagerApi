using System;
using System.Linq;
using ChurchManager.Domain.Common;
using CodeBoss.MultiTenant;

namespace ChurchManager.Application.Tests.Common
{
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
}