#region

using ChurchManager.Domain.Common;
using ChurchManager.Infrastructure.Abstractions.Configuration;
using ChurchManager.Infrastructure.Persistence.Contexts.Factory;
using ChurchManager.Infrastructure.Persistence.Seeding;
using ChurchManager.Persistence.Shared;
using CodeBoss.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Npgsql;

#endregion

namespace ChurchManager.Infrastructure.Persistence;

public class DbTenantMigrationHostedService :  IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DbTenantMigrationHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
            
        var dbOptions = _serviceProvider.GetRequiredService<IOptions<DbOptions>>().Value;
        if (!dbOptions.Migrate)
        {
            Console.WriteLine("[X] Migrations disabled.");
            return;
        }
        Console.WriteLine("[✔️] Migrations enabled.");
            
        // Try seeding Tenants
        TryMigrateAndSeedMasterDatabase(ct, scope);
        
        // Start Migration for each tenant in the current sub-domain
        var tenantsProvider = scope.ServiceProvider.GetRequiredService<ITenantsProvider<TenantConfiguration>>();
        var tenants = tenantsProvider.Tenants();
        IEnumerable<Task> tasks = tenants.Select(tenant => MigrateTenantDatabase(tenant, tenantsProvider, ct));

        Console.WriteLine("> Starting parallel execution of pending migrations...");
        await Task.WhenAll(tasks);
    }
        

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task MigrateTenantDatabase(
        ITenant tenant,
        ITenantsProvider<TenantConfiguration> tenants,
        CancellationToken ct = default)
    {
        try
        {
            await using var dbContext = DbContextFactory.Create(tenant.ConnectionString, tenants);
            if ((await dbContext.Database.GetPendingMigrationsAsync(ct)).Any())
            {
                Console.WriteLine($"*** Beginning migration for: [{tenant.Name}]");
                    
                await dbContext.Database.MigrateAsync(ct);
                
                // ------------------------------------------------------------------------------
                // Fixes issues with PostgreSQL not reloading types after migration e.g. hstore extension
                // https://github.com/npgsql/efcore.pg/issues/292#issuecomment-388608426
                await dbContext.Database.OpenConnectionAsync(ct);
                await ((NpgsqlConnection)dbContext.Database.GetDbConnection()).ReloadTypesAsync(ct);
                await dbContext.Database.CloseConnectionAsync();
                // ------------------------------------------------------------------------------
                
                Console.WriteLine($"*** Completed migration for: [{tenant.Name}]");
            }
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error occurred during migration: {e.Message} --> [{tenant.Name}]");
            throw;
        }
    }
    
    private async Task MigrateMasterDatabase(IServiceScope scope, CancellationToken ct = default)
    {
        try
        {
            var connectionString = scope.ServiceProvider.GetRequiredService<IConfiguration>().GetConnectionString("MasterDatabase");
            await using var dbContext = DbContextFactory.Create(connectionString!);
            if ((await dbContext.Database.GetPendingMigrationsAsync(ct)).Any())
            {
                Console.WriteLine($"*** Beginning [MasterDb] migration.");
                    
                await dbContext.Database.MigrateAsync(ct);
                
                Console.WriteLine($"*** Completed [MasterDb] migration.");
            }
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error occurred during migration: {e.Message} --> [MasterDb]");
            throw;
        }
    }
        
    private void TryMigrateAndSeedMasterDatabase(CancellationToken ct, IServiceScope scope)
    {
        MigrateMasterDatabase(scope, ct).Wait(ct);
        
        var tenantSeeder = scope.ServiceProvider.GetService<TenantsDbFakeSeedInitializer>();
        tenantSeeder?.InitializeAsync().Wait(ct);
        Console.WriteLine("Attempting to Seed Tenants... " + (tenantSeeder == null ? "[No Seeder Registered]" : "[Succeeded]"));
    }
}