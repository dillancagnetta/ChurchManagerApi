using ChurchManager.Domain.Common;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Permissions;

public class EntityPermission : AuditableEntity<int>, IAggregateRoot<int>
{
    // Link to UserLoginRole
    public int UserLoginRoleId { get; set; }
    public virtual UserLoginRole Role { get; set; }
    
    public bool IsSystem { get; set; }
    
    // What entity this permission is for
    public string EntityType { get; set; }
    
    // Explicit IDs (can be null if using dynamic scope)
    public ICollection<int> EntityIds { get; set; }
    
    // Dynamic scope properties
    public bool IsDynamicScope { get; set; }
    public string ScopeType { get; set; }  // e.g., "ChurchGroup", "Church"
    public int ScopeId { get; set; }       // e.g., ChurchGroupId or ChurchId
    
    // Permissions
    public bool CanView { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanManageUsers { get; set; }

    public static EntityPermission SystemAdminPermission =>
        new()
        {
            IsSystem = true,
            CanView = true,
            CanEdit = true,
            CanDelete = true,
            CanManageUsers = true,
            IsDynamicScope = true,
            ScopeType = SystemAdminPermissionScope
        };
    
    public const string SystemAdminPermissionScope = "SystemAdmin";
}