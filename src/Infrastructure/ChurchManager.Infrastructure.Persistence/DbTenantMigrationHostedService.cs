#region

using ChurchManager.Infrastructure.Persistence.Contexts.Factory;
using ChurchManager.Persistence.Shared;
using CodeBoss.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Npgsql;

#endregion

namespace ChurchManager.Infrastructure.Persistence
{
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

            var tenantProvider = scope.ServiceProvider.GetRequiredService<ITenantProvider>();
                
            var tenants = tenantProvider.Tenants();

            IEnumerable<Task> tasks = tenants.Select(tenant => MigrateTenantDatabase(tenant, tenantProvider, ct));

            Console.WriteLine("> Starting parallel execution of pending migrations...");
            await Task.WhenAll(tasks);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task MigrateTenantDatabase(ITenant tenant, ITenantProvider provider, CancellationToken ct = default)
        {
            try
            {
                Console.WriteLine($"*** Beginning migration: [{tenant.Name}]");

                using var context = DbContextFactory.Create(tenant.ConnectionString, provider);
                await context.Database.MigrateAsync(ct);
                
                // Fixes issues with PostgreSQL not reloading types after migration e.g. hstore extension
                // https://github.com/npgsql/efcore.pg/issues/292#issuecomment-388608426
                await context.Database.OpenConnectionAsync(ct);
                await ((NpgsqlConnection)context.Database.GetDbConnection()).ReloadTypesAsync(ct);
                // ------------------------------------------------------------------------------
                
                Console.WriteLine($"*** Completed migration: [{tenant.Name}]");
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error occurred during migration: {e.Message} --> [{tenant.Name}]");
                throw;
            }
        }
    }
}
