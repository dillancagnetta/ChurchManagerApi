using ChurchManager.Domain.Common;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Infrastructure.Persistence.Seeding;

public class TenantsDbFakeSeedInitializer(IServiceScopeFactory scopeFactory) : IInitializer
{
    public int OrderNumber { get; } = -1;
    
    public async Task InitializeAsync()
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MasterDbContext>();
        
        if (!await dbContext.Tenants.AnyAsync())
        {
            var tenant1 = new TenantConfiguration
            {
                Name = "Tenant1",
                ConnectionString = "Server=localhost;Database=churchmanager_db;Port=5432;User Id=admin;password=P455word1",
                Email = "tenant1@example.com",
                Subdomain = "localhost",
            };

            await dbContext.Tenants.AddRangeAsync(tenant1);
            await dbContext.SaveChangesAsync();
        }
    }
}