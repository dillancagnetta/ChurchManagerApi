using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Parameters;
using ChurchManager.Domain.Shared;
using ChurchManager.SharedKernel.Wrappers;
using Codeboss.Results;
using MediatR;

namespace ChurchManager.Features.UserLogins.Queries;

public record BrowsePermissionsQuery : QueryParameter,  IRequest<PagedResponse<PermissionViewModel>>
{
    public string EntityType { get; set; }
    public string ScopeType { get; set; }
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
    public string EntityType { get; set; }
    public string PermissionType { get; set; }
    public IEnumerable<int> EntityIds { get; set; }
    public bool CanView { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanManageUsers { get; set; }
    public int? ScopeId { get; set; }
    public string ScopeType { get; set; }
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
        return new ApiResponse {Succeeded = true};
    }
}