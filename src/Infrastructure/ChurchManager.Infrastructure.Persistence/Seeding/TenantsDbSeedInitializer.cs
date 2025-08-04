using ChurchManager.Domain.Common;
using ChurchManager.Infrastructure.Abstractions.Configuration;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.AspNetCore.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Infrastructure.Persistence.Seeding;

public class TenantsDbSeedInitializer(IServiceScopeFactory scopeFactory) : IInitializer
{
    public int OrderNumber { get; } = -1;
    
    public async Task InitializeAsync()
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MasterDbContext>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var envConfig = scope.ServiceProvider.GetRequiredService<IEnvironmentConfig>();
        
        if (!await dbContext.Tenants.AnyAsync())
        {
            var isDevelopment = envConfig.IsDevelopment;
            
            var tenant1 = new TenantConfiguration
            {
                Name = SeedingConstants.TestTenantName,
                ConnectionString = configuration.GetConnectionString("DefaultConnection")!,
                Email = "tenant1@example.com",
                Subdomain = isDevelopment ? "localhost:4200" : "demo",
                ApiUrl =  isDevelopment ?"http://localhost:5001" : "https://demo.churchmanager.io",
            };

            await dbContext.Tenants.AddRangeAsync(tenant1);
            await dbContext.SaveChangesAsync();
        }
    }
}