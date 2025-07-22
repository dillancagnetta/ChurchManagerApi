using System.ComponentModel.DataAnnotations;
using ChurchManager.Persistence.Shared;
using CodeBoss.MultiTenant;

namespace ChurchManager.Domain.Common;

public class TenantConfiguration: AuditableEntity<int>, ITenant
{
    [Required, MaxLength(50)]public required string Name { get; set; }
    [Required, MaxLength(100)] public required string Email { get; set; }
    [MaxLength(20)] public string? Phone { get; set; }
    [Required, MaxLength(50)] public required string Subdomain { get; set; }
    [Required, MaxLength(200)] public required string ApiUrl { get; set; }
    public string ConnectionString { get; set; }
}
