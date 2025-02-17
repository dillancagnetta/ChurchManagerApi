using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.Security;
using ChurchManager.Persistence.Shared;
using CodeBoss.Extensions;
using Codeboss.Types;

namespace ChurchManager.Domain.Common;

public class UserLogin : Entity<int>, IAggregateRoot<int>
{
    public Guid Id { get; set; }
    [Required]
    [MaxLength(255)]
    public string Username { get; set; }
    [MaxLength(128)]
    public string Password { get; set; }
    public string RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    
    // Many-to-many relationship with Role through UserRoleAssignment
    public virtual ICollection<UserRoleAssignment> UserRoles { get; set; } = new List<UserRoleAssignment>();

    public int PersonId { get; set; }

    [Required]
    [MaxLength(50)]
    [DefaultValue("Tenant1")]
    public string Tenant { get; set; }

    #region Navigation Properties

    public virtual Person Person { get; set; }

    #endregion

    public void ClearRefreshTokenHistory()
    {
        RefreshToken = null;
        RefreshTokenExpiryTime = null;  
    }

    public void AddUserLoginRole(UserRoleAssignment assignment)
    {
        UserRoles.Add(assignment);
    }
    
    public void AddUserLoginRole(UserLoginRole role)
    {
        UserRoles.Add(new UserRoleAssignment() {UserLogin = this, Role = role });
    }
}

public class UserLoginRole : AuditableEntity<int>, IAggregateRoot<int>
{
    [MaxLength(50)]
    public string Name { get; set; }
    
    public string Description { get; set; }

    public bool IsSystem { get; set; }
    
    // Many-to-many relationship with UserLogin through UserRoleAssignment
    public virtual ICollection<UserRoleAssignment> UserAssignments { get; set; } = new List<UserRoleAssignment>();

// Many-to-many relationship with Permission through RolePermissionAssignment
    public virtual ICollection<RolePermissionAssignment> PermissionAssignments { get; set; } = new List<RolePermissionAssignment>();

    public UserLoginRole(string name)
    {
        Name = name;
        RecordStatus = Common.RecordStatus.Active.Value;
    }
    
    public UserLoginRole(string name, string description) : this(name)
    {
        Description = description;
    }

    public UserLoginRole()
    {
        RecordStatus = Common.RecordStatus.Active.Value;
    }

    public static UserLoginRole SystemAdminRole =>
        new()
        {
            Name = SystemAdminRoleName,
            Description = "System Administrator Role",
            IsSystem = true
        };
    
    public const string SystemAdminRoleName = "System Admin";
}

// Junction table for many-to-many relationship between UserLogin and Role
public class UserRoleAssignment : AuditableEntity<int>, IAggregateRoot<int>
{
    public Guid UserLoginId { get; set; }
    public virtual UserLogin UserLogin { get; set; }
    
    public int UserLoginRoleId { get; set; }
    public virtual UserLoginRole Role { get; set; }
}

// Junction table for many-to-many relationship between Role and Permission
public class RolePermissionAssignment : AuditableEntity<int>
{
    public int RoleId { get; set; }
    public virtual UserLoginRole Role { get; set; }
    
    public int EntityPermissionId { get; set; }
    public virtual EntityPermission Permission { get; set; }
}