using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Infrastructure.Abstractions.MultiTenancy;

public interface ITenantDbContextFactory
{
    IChurchManagerDbContext CreateDbContext(int? tenantId);
}