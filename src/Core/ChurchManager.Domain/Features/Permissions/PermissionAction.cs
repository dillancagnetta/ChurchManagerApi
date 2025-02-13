using ChurchManager.Domain.Features.Communications;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Permissions;


public class PermissionAction : Enumeration<PermissionAction, string>
{
    public PermissionAction(string value) => Value = value;
    
    public PermissionAction() { Value = "View"; }

    public static PermissionAction View = new("View");
    public static PermissionAction Edit = new("Edit");
    public static PermissionAction Delete = new("Delete");
    public static PermissionAction ManageUsers = new("ManageUsers");

    public static implicit operator PermissionAction(string value) => new(value);
}