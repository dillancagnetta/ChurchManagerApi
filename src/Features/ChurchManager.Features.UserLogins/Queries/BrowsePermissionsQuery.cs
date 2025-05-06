using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Parameters;
using ChurchManager.SharedKernel.Wrappers;
using Codeboss.Results;
using MediatR;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Features.UserLogins.Queries;

public record BrowsePermissionsQuery : QueryParameter,  IRequest<PagedResponse<PermissionViewModel>>
{
    public string? EntityType { get; set; }
    public string? ScopeType { get; set; }
    public int? EntityId { get; set; }
    public bool? IsDynamicScope { get; set; }
}

public class PermissionsQueryHandler(ISecurityService service) : IRequestHandler<BrowsePermissionsQuery, PagedResponse<PermissionViewModel>>
{
    public async Task<PagedResponse<PermissionViewModel>> Handle(BrowsePermissionsQuery request, CancellationToken ct)
    {
        var result = await service.BrowsePermissionsAsync(request, request.EntityType, request.ScopeType, request.EntityId, request.IsDynamicScope, ct);
        
        return result;
    }
}

/*
 * ---------------------------------------------------------
 */
 
public record CreatePermissionCommand : IRequest<ApiResponse>
{
    public string? EntityType { get; set; }
    public string? PermissionType { get; set; }
    public IEnumerable<int> EntityIds { get; set; } = [];
    public bool CanView { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanManageUsers { get; set; }
    public int? ScopeId { get; set; }
    public string? ScopeType { get; set; }
}

public class CreatePermissionHandler(ISecurityService service) : IRequestHandler<CreatePermissionCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(CreatePermissionCommand command, CancellationToken ct)
    {
        var operation = await service.CreatePermissionAsync(
            command.PermissionType, 
            command.EntityType, 
            command.EntityIds,
            command.ScopeType, 
            command.ScopeId, 
            command.CanView, 
            command.CanEdit, 
            command.CanDelete, 
            command.CanManageUsers, ct);
        
        return new ApiResponse {Succeeded = operation.IsSuccess};
    }
}

/*
 * ---------------------------------------------------------
 */

public record AddPermissionsToRoleCommand(int UserLoginRoleId, int[] PermissionIds, bool? IsAllSelected = null) : IRequest<ApiResponse>;

public class AddPermissionsToRoleHandler(ISecurityService service) : IRequestHandler<AddPermissionsToRoleCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(AddPermissionsToRoleCommand command, CancellationToken ct)
    {
        OperationResult operation = await service.AddPermissionsToRoleAsync(command.UserLoginRoleId, command.PermissionIds, command.IsAllSelected, ct);
        return ApiResponse.FromOperation(operation);
    }
}

/*
 * ---------------------------------------------------------
 */

public record AddRoleToUserCommand(int UserLoginRoleId, Guid UserLoginId) : IRequest<ApiResponse>;

public class AddRoleToUserCommandHandler(ISecurityService service) : IRequestHandler<AddRoleToUserCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(AddRoleToUserCommand command, CancellationToken ct)
    {
        var operation = await service.AddRoleToUserAsync(command.UserLoginId, command.UserLoginRoleId, ct);
        return ApiResponse.FromOperation(operation);
    }
}

/*
 * ---------------------------------------------------------
 */

public record RemovePermissionFromRoleCommand(int UserLoginRoleId, int PermissionId) : IRequest<ApiResponse>;

public class RemovePermissionsFromRoleCommandHandler(ISecurityService service) : IRequestHandler<RemovePermissionFromRoleCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(RemovePermissionFromRoleCommand command, CancellationToken ct)
    {
        OperationResult operation = await service.RemovePermissionFromRoleAsync(command.UserLoginRoleId, command.PermissionId, ct);
        return ApiResponse.FromOperation(operation);
    }
}

/*
 * ---------------------------------------------------------
 */

public record RemoveRoleFromUserCommand(int UserLoginRoleId, Guid UserLoginId) : IRequest<ApiResponse>;

public class RemoveRoleFromUserCommandHandler(ISecurityService service) : IRequestHandler<RemoveRoleFromUserCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(RemoveRoleFromUserCommand command, CancellationToken ct)
    {
        var operation = await service.RemoveRoleFromUserAsync(command.UserLoginId, command.UserLoginRoleId, ct);
        return ApiResponse.FromOperation(operation);
    }
}

/*
 * ---------------------------------------------------------
 */

public record TogglePermissionStatusForRoleCommand(int UserLoginRoleId, int PermissionId) : IRequest<ApiResponse>;

public class TogglePermissionStatusForRoleCommandHandler(ISecurityService service) : IRequestHandler<TogglePermissionStatusForRoleCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(TogglePermissionStatusForRoleCommand command, CancellationToken ct)
    {
        OperationResult operation = await service.TogglePermissionStatusForRoleCommandAsync(command.UserLoginRoleId, command.PermissionId, ct);
        return ApiResponse.FromOperation(operation);
    }
}

/*
 * ---------------------------------------------------------
 */

public record ToggleRoleStatusForUserCommand(int UserLoginRoleId, Guid UserLoginId) : IRequest<ApiResponse>;

public class ToggleRoleStatusForUserHandler(ISecurityService service) : IRequestHandler<ToggleRoleStatusForUserCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(ToggleRoleStatusForUserCommand command, CancellationToken ct)
    {
        var operation = await service.ToggleRoleStatusForUserCommandAsync(command.UserLoginId, command.UserLoginRoleId, ct);
        return ApiResponse.FromOperation(operation);
    }
}

/*
 * ---------------------------------------------------------
 */

public record AddRoleCommand(string Name, string? Description) : IRequest<ApiResponse>;

public class AddRoleCommandHandler(ISecurityService service) : IRequestHandler<AddRoleCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(AddRoleCommand command, CancellationToken ct)
    {
        var operation = await service.AddRoleAsync(command.Name, command.Description, ct);
        return ApiResponse.FromOperation(operation);
    }
}
