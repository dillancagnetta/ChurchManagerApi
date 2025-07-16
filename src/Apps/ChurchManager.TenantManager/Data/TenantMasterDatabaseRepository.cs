using ChurchManager.Domain.Common;
using ChurchManager.Infrastructure.Persistence.Contexts.Factory;
using CodeBoss.MultiTenant;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace ChurchManager.TenantManager.Data;

/*
 * NOT USED: left here for reference or future use.
 */
public class TenantMasterDatabaseRepository(
    IConfiguration configuration,
    IServiceProvider serviceProvider,
    ITenantsProvider<TenantConfiguration> tenantsProvider,
    ILogger<TenantMasterDatabaseRepository> logger)
{
    public async Task<string> CreateTenantDatabase(string tenantName)
    {
        var masterConnectionString = configuration.GetConnectionString("MasterDatabase");
        var databaseName = $"{tenantName}_db";
        
        try
        {
            // Create the database
            await CreateDatabase(masterConnectionString!, databaseName);
            
            // Get the connection string for the new database
            var tenantConnectionString = BuildTenantConnectionString(databaseName);
            
            // Run migrations
            await RunMigrations(tenantConnectionString);
            
            return tenantConnectionString;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error creating database for tenant {tenantName}");
            throw;
        }
    }
    
    private async Task CreateDatabase(string masterConnectionString, string databaseName)
    {
        using var connection = new NpgsqlConnection(masterConnectionString);
        await connection.OpenAsync();
        
        // Check if database already exists
        var checkCommand = new NpgsqlCommand(
            $"SELECT 1 FROM pg_database WHERE datname = '{databaseName}'", 
            connection);
        var exists = await checkCommand.ExecuteScalarAsync();
        
        if (exists == null)
        {
            var createCommand = new NpgsqlCommand($"CREATE DATABASE \"{databaseName}\"", connection);
            await createCommand.ExecuteNonQueryAsync();
            logger.LogInformation($"Database {databaseName} created successfully");
        }
    }
    
    private string BuildTenantConnectionString(string tenantName)
    {
        var builder = new NpgsqlConnectionStringBuilder(configuration.GetConnectionString("MasterDatabase"))
        {
            Database = $"churchmanager_{tenantName}_db"
        };
        return builder.ToString();
        return tenantsProvider.Get(tenantName).ConnectionString;
    }
    
    private async Task RunMigrations(string connectionString)
    {
        await using var dbContext = DbContextFactory.Create(connectionString, tenantsProvider);
        
        /*var optionsBuilder = new DbContextOptionsBuilder<ChurchManagerDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        
        using var context = new ChurchManagerDbContext(optionsBuilder.Options);
        await context.Database.MigrateAsync();*/
        logger.LogInformation("Migrations completed successfully");
    }
    
    public async Task DeleteTenantDatabase(string tenantId)
    {
        var masterConnectionString = configuration.GetConnectionString("MasterDatabase");
        var databaseName = $"{tenantId}_db";
        
        using var connection = new NpgsqlConnection(masterConnectionString);
        await connection.OpenAsync();
        
        // Terminate all connections to the database
        var terminateCommand = new NpgsqlCommand($@"
            SELECT pg_terminate_backend(pg_stat_activity.pid)
            FROM pg_stat_activity
            WHERE pg_stat_activity.datname = '{databaseName}'
            AND pid <> pg_backend_pid()", connection);
        await terminateCommand.ExecuteNonQueryAsync();
        
        // Drop the database
        var dropCommand = new NpgsqlCommand($"DROP DATABASE IF EXISTS \"{databaseName}\"", connection);
        await dropCommand.ExecuteNonQueryAsync();
        
        logger.LogInformation($"Database {databaseName} deleted successfully");
    }
}