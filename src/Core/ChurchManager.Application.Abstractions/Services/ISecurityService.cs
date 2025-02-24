using ChurchManager.Domain.Shared;
using ChurchManager.SharedKernel.Wrappers;
using Codeboss.Results;
using Convey.CQRS.Queries;

namespace ChurchManager.Application.Abstractions.Services;

public interface ISecurityService
{
    Task<IEnumerable<UserLoginViewModel>> UserLoginsAsync(string searchTerm, CancellationToken ct = default);
    Task<IEnumerable<UserLoginRoleViewModel>> UserLoginRolesAsync(string searchTerm = null, IEnumerable<int> excludeIds = null, CancellationToken ct = default);
    Task<PagedResponse<PermissionViewModel>> BrowsePermissionsAsync(IPagedQuery query, string entityType, string scopeType, int? entityId = null, bool? isDynamicScope = null,
        CancellationToken ct = default);
    Task<OperationResult> CreatePermissionAsync(string permissionType, string entityType, IEnumerable<int> entityIds, string scopeType, int? scopeId, bool canView, bool canEdit, bool canDelete, bool canManageUsers, CancellationToken ct = default);
    Task<IEnumerable<PermissionViewModel>> EntityPermissionsAsync(IEnumerable<int> excludeIds, int? UserLoginRoleId = null, CancellationToken ct = default);
}