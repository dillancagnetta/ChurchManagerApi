using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.Permissions;
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
    public virtual List<UserLoginRole> Roles { get; set; } = new(0);

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
}

public class UserLoginRole : AuditableEntity<int>, IAggregateRoot<int>
{
    [MaxLength(50)]
    public string Name { get; set; }
    
    public string Description { get; set; }

    public bool IsSystem { get; set; }
    
    // Link to UserLogin
    public Guid UserLoginId { get; set; }
    
    public virtual UserLogin UserLogin { get; set; }

    public virtual ICollection<EntityPermission> Permissions { get; set; } = new List<EntityPermission>(0);

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
            IsSystem = true,
            Permissions = [EntityPermission.SystemAdminPermission]
        };
    
    public const string SystemAdminRoleName = "System Admin";
}