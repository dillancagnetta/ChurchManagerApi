namespace ChurchManager.Domain.Shared;
public record UserLoginViewModel
{
    public Guid Id { get; set; }
    public string Username { get; set; } 
    public string RecordStatus { get; set; }
    public PersonViewModelBasic Person { get; set; }
    public IEnumerable<UserLoginRoleViewModel> Roles { get; set; }
};

public class UserLoginRoleViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsSystem { get; set; }
    public string RecordStatus { get; set; }
    public List<UserLoginBasicViewModel> UserLogins { get; set; }
    public List<PermissionViewModel> Permissions { get; set; }
}

public class UserLoginBasicViewModel
{
    public Guid Id { get; set; }
    public string Username { get; set; }
}

public class PermissionViewModel
{
    public int Id { get; set; }
    public string EntityType { get; set; }
    public string ScopeType { get; set; }
    public bool IsSystem { get; set; }
    public string RecordStatus { get; set; }
    public ICollection<int> EntityIds { get; set; }
    public bool CanView { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanManageUsers { get; set; }
}

