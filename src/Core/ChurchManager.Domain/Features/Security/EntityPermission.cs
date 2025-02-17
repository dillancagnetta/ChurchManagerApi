using ChurchManager.Domain.Common;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Domain.Features.Security;

/*
 
 // Example: Create a reusable permission
var churchViewPermission = new Permission 
{
    EntityType = "Church",
    CanView = true,
    IsDynamicScope = true,
    ScopeType = "ChurchGroup"
};

// Create a role and assign the permission
var pastorRole = new Role("Pastor", "Church Pastor Role");
var rolePermission = new RolePermissionAssignment 
{
    Role = pastorRole,
    Permission = churchViewPermission
};

// Assign the role to a user
var userRoleAssignment = new UserRoleAssignment 
{
    UserLogin = user,
    Role = pastorRole
};
 
 */
public class EntityPermission : AuditableEntity<int>, IAggregateRoot<int>
{
    // Link to UserLoginRole
    // Many-to-many relationship with Role through RolePermissionAssignment
    public virtual ICollection<RolePermissionAssignment> RoleAssignments { get; set; } = new List<RolePermissionAssignment>();
    
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

