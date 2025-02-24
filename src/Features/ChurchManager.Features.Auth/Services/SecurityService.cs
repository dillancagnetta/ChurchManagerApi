using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Security;
using ChurchManager.Domain.Features.Security.Services;
using ChurchManager.Domain.Features.Security.Specifications;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Common;
using ChurchManager.SharedKernel.Wrappers;
using CodeBoss.Extensions;
using Codeboss.Results;
using Convey.CQRS.Queries;

namespace ChurchManager.Features.Auth.Services;

public class SecurityService(IPermissionContext permissions,
    IGenericDbRepository<UserLogin> userLoginDb,
    IReadDbRepository<UserLoginRole> rolesDb,
    IGenericDbRepository<EntityPermission> permissionsDb,
    ICognitoCurrentUser currentUser
    ) : ISecurityService
{
    public async Task<IEnumerable<UserLoginViewModel>> UserLoginsAsync(string searchTerm, CancellationToken ct = default)
    {
        var spec = new UserLoginsSpecification(searchTerm);
        
        var vm = await userLoginDb.ListAsync(spec, ct);
        return vm;
    }

    public async Task<IEnumerable<UserLoginRoleViewModel>> UserLoginRolesAsync(string searchTerm = null, IEnumerable<int> excludeIds = null, CancellationToken ct = default)
    {
        var spec = new UserLoginRolesSpecification(searchTerm, excludeIds:excludeIds);
        
        var vm = await rolesDb.ListAsync(spec, ct);
            
        return vm;
    }

    public async Task<PagedResponse<PermissionViewModel>> BrowsePermissionsAsync(IPagedQuery query, string entityType, string scopeType, int? entityId = null, bool? isDynamicScope = null,
        CancellationToken ct = default)
    {
        var spec = new BrowsePermissionSpecification(entityType, scopeType, entityId, isDynamicScope);
        
        var pagedResult = await permissionsDb.BrowseAsync(query, spec, ct);
        
        return new PagedResponse<PermissionViewModel>(pagedResult);
    }

    public async Task<OperationResult> CreatePermissionAsync(string permissionType, string entityType, IEnumerable<int> entityIds, string scopeType, int? scopeId,
        bool canView, bool canEdit, bool canDelete, bool canManageUsers, CancellationToken ct = default)
    {
        var permission = new EntityPermission
        {
            EntityType = entityType.Replace(" ", ""),
            IsDynamicScope = permissionType.Equals("dynamic", StringComparison.InvariantCultureIgnoreCase),
            EntityIds = !entityIds.IsNullOrEmpty() ? entityIds.ToList() : null,
            ScopeType = scopeType.Replace(" ", ""),
            ScopeId = scopeId,
            CanView = canView,
            CanEdit = canEdit,
            CanDelete = canDelete,
            CanManageUsers = canManageUsers,
            IsSystem = false
        };
        await permissionsDb.AddAsync(permission, ct);
        var result = await permissionsDb.SaveChangesAsync(ct);
        
        return result == 1 ? OperationResult.Success() : OperationResult.Fail("Failed to create permission");
    }

    public async Task<IEnumerable<PermissionViewModel>> EntityPermissionsAsync(IEnumerable<int> excludeIds, int? userLoginRoleId = null, CancellationToken ct = default)
    {
        var spec = new EntityPermissionsSpecification(excludeIds, userLoginRoleId);
        
        var vm = await permissionsDb.ListAsync(spec, ct);
        
        return vm;
    }
}