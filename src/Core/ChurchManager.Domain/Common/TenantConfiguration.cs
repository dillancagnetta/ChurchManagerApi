using ChurchManager.Persistence.Shared;
using CodeBoss.MultiTenant;

namespace ChurchManager.Domain.Common;

public class TenantConfiguration: AuditableEntity<int>, ITenant
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public string? Subdomain { get; set; }
    public string ConnectionString { get; set; }
}
