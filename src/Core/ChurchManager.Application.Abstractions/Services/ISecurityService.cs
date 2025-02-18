using ChurchManager.Domain.Shared;
using ChurchManager.SharedKernel.Wrappers;
using Convey.CQRS.Queries;

namespace ChurchManager.Application.Abstractions.Services;

public interface ISecurityService
{
    Task<IEnumerable<UserLoginViewModel>> UserLoginsAsync(string searchTerm, CancellationToken ct = default);
    Task<IEnumerable<UserLoginRoleViewModel>> UserLoginRolesAsync(string searchTerm, CancellationToken ct = default);
    Task<PagedResponse<PermissionViewModel>> BrowsePermissionsAsync(IPagedQuery query, string searchTerm, int? entityId = null, bool? isDynamicScope = null,
        CancellationToken ct = default);
}