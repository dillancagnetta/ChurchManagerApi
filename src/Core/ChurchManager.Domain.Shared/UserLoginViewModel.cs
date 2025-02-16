namespace ChurchManager.Domain.Shared;
public record UserLoginViewModel
{
    public Guid Id { get; set; }
    public string Username { get; set; } 
    public string RecordStatus { get; set; }
    public PersonViewModelBasic Person { get; set; }
    public IEnumerable<UserLoginRoleViewModel> Roles { get; set; }
};

public record UserLoginRoleViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsSystem { get; set; }
    public Guid UserLoginId { get; set; }
    public string RecordStatus { get; set; }
    public IEnumerable<PermissionViewModel> Permissions { get; set; }
};

public record PermissionViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string EntityType { get; set; }
    public ICollection<int> EntityIds { get; set; }
    
    // Dynamic scope properties
    public bool IsDynamicScope { get; set; }
    public string ScopeType { get; set; }  // e.g., "ChurchGroup", "Church"
    public int ScopeId { get; set; }      
    public bool IsSystem { get; set; }
    public string RecordStatus { get; set; }
    
    // Permissions
    public bool CanView { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanManageUsers { get; set; }
};