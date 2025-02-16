using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.UserLogins.Commands;

public record AddUserLoginRoleCommand: IRequest<ApiResponse>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid UserLoginId { get; set; }
    public IEnumerable<AddPermissionCommand> Permissions { get; set; }
};