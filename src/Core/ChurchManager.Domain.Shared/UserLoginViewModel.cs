namespace ChurchManager.Domain.Shared;
public record UserLoginViewModel
{
    public Guid Id { get; set; }
    public required string Username { get; set; } 
    public string RecordStatus { get; set; } = "Active";
    public PersonViewModelBasic? Person { get; set; }
    public IEnumerable<UserLoginRoleViewModel> Roles { get; set; } = [];
};

public class UserLoginRoleViewModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsSystem { get; set; }
    public string RecordStatus { get; set; } = "Active";
    public IEnumerable<UserLoginBasicViewModel> UserLogins { get; set; } = [];
    public IEnumerable<PermissionViewModel> Permissions { get; set; } = [];
}

public class UserLoginBasicViewModel
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
}

public class PermissionViewModel
{
    public int Id { get; set; }
    public int? ScopeId { get; set; }
    public string? EntityType { get; set; }
    public string? ScopeType { get; set; }
    public bool IsSystem { get; set; }
    public string? RecordStatus { get; set; }
    public IEnumerable<int> EntityIds { get; set; } = [];
    public bool IsDynamicScope { get; set; }
    public bool CanView { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanManageUsers { get; set; }
    
    // Resolved Names
    public string? ScopeName { get; set; }
    public IEnumerable<string> EntityNames { get; set; } = [];
}

