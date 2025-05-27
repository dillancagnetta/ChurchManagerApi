
using ChurchManager.Domain.Common;

namespace ChurchManager.TenantManager;

public interface ITenantService
{
    string CurrentTenantId();
    Task<TenantConfiguration?>? TenantConfigurationAsync();
    Task<TenantConfiguration?>? TenantConfigurationAsync(string tenantName);
}