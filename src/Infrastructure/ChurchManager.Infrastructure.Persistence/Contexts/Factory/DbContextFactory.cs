#region

using ChurchManager.Domain.Common;
using CodeBoss.MultiTenant;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Contexts.Factory
{
    public static class DbContextFactory
    {
        public static ChurchManagerDbContext Create(string connectionString, ITenantsProvider<TenantConfiguration> tenants) =>
            new (CreateDefaultDbContextOptions(connectionString), tenants);

        public static DbContextOptions<ChurchManagerDbContext> CreateDefaultDbContextOptions(string connectionString) =>
            new DbContextOptionsBuilder<ChurchManagerDbContext>()
                .UseNpgsql(connectionString)
                .Options;
        
        public static MasterDbContext Create(string connectionString) =>
            new (new DbContextOptionsBuilder<MasterDbContext>()
                .UseNpgsql(connectionString)
                .Options);
    }
}
