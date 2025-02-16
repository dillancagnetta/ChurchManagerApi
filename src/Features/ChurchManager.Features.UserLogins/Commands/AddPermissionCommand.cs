using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.UserLogins.Commands;

public record AddPermissionCommand: IRequest<ApiResponse>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid? UserLoginRoleId { get; set; }
    public bool CanView { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanManageUsers { get; set; }
};