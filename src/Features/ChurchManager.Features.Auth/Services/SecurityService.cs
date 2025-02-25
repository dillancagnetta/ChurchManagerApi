using System.Linq.Dynamic.Core;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ChurchManager.Features.Auth.Services;

public class SecurityService(IPermissionContext permissions,
    IGenericDbRepository<UserLogin> userLoginDb,
    IReadDbRepository<UserLoginRole> rolesDb,
    IGenericDbRepository<UserLoginRole> rolesWriteDb,
    IGenericDbRepository<EntityPermission> permissionsDb,
    ICognitoCurrentUser currentUser,
    IEntityPermissionsResolver permissionsResolver
    ) : ISecurityService
{
    public async Task<IEnumerable<UserLoginViewModel>> UserLoginsAsync(string searchTerm, CancellationToken ct = default)
    {
        var spec = new UserLoginsSpecification(searchTerm);
        
        var vm = await userLoginDb.ListAsync(spec, ct);
        return vm;
    }

    public async Task<IEnumerable<UserLoginRoleViewModel>> UserLoginRolesAsync(string searchTerm = null, IEnumerable<int> excludeIds = null, Guid? userLoginId = null, CancellationToken ct = default)
    {
        var spec = new UserLoginRolesSpecification(searchTerm, excludeIds:excludeIds, userLoginId:userLoginId);
        
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

        var entityPermissions = vm
            .Where(x => !x.IsDynamicScope)
            .GroupBy(x => x.EntityType)
            .ToDictionary(
                g => g.Key,
                g => g.SelectMany(p => p.EntityIds).ToArray()
            );;

        var dynamicPermissions = vm
            .Where(x => x.IsDynamicScope)
            .GroupBy(x => x.ScopeType)
            .ToDictionary(
                g => g.Key,
                g => g.Select(p => p.ScopeId).Where(id => id.HasValue).Select(id => id.Value).ToArray()
            );

        var resolvedPermissionNames = new List<(string Key, IReadOnlyList<SelectItemViewModel> Items)>();
        foreach (var permission in entityPermissions)
        {
            resolvedPermissionNames.Add((permission.Key, await permissionsResolver.ResolveAsync(permission.Key, permission.Value, ct)));
        }
        foreach (var permission in dynamicPermissions)
        {
            resolvedPermissionNames.Add((permission.Key, await permissionsResolver.ResolveAsync(permission.Key, permission.Value, ct)));
        }

        var x = resolvedPermissionNames
            .GroupBy(x => x.Key);
        
        var resolvedPermissions =  resolvedPermissionNames
            .GroupBy(x => x.Key)
            .ToDictionary(
                g => g.Key,
                g => g.SelectMany(p => p.Items)
                    .GroupBy(x => x.Id)
                    .ToDictionary
                        (x => x.Key,
                            x => x.Select(x => x.Name))
        );
        
        foreach (var viewModel in vm)
        {
            if (viewModel.IsDynamicScope)
            {
                var scopeResolved = resolvedPermissions[viewModel.ScopeType.Replace(" ", "")];
                var scopeName = scopeResolved[viewModel.ScopeId.Value];
                viewModel.ScopeName = scopeName.FirstOrDefault();
            }
            else
            {
                var entityResolved = resolvedPermissions[viewModel.EntityType.Replace(" ", "")];
                var entityNames = viewModel.EntityIds.SelectMany(id => entityResolved[id]).ToList();
                viewModel.EntityNames = entityNames;
            }
        }
        
        return vm;
    }

    public async Task<OperationResult> AddRoleToUserAsync(Guid userLoginId, int userLoginRoleId, CancellationToken ct = default)
    {
        var userLogin = await userLoginDb.GetByIdAsync(userLoginId, ct);
        if (userLogin is null)  return OperationResult.Fail("User login not found");
        
        userLogin.AddUserLoginRole(userLoginRoleId);
        return new OperationResult(await userLoginDb.SaveChangesAsync(ct) > 0);
    }

    public async Task<OperationResult> AddPermissionsToRoleAsync(int userLoginRoleId, int[] permissionIds, bool? isAllSelected,
        CancellationToken ct = default)
    {
        var role = await rolesWriteDb.GetByIdAsync(userLoginRoleId, ct);
        if (role is null)  return OperationResult.Fail("Role not found");
        
        if (isAllSelected is true)
        {
            permissionIds = await permissionsDb.Queryable().AsNoTracking().Select(x => x.Id).ToArrayAsync(ct);
        }

        if (!permissionIds.IsNullOrEmpty())
        {
            role.AssignPermissions(permissionIds);
            return new OperationResult(await userLoginDb.SaveChangesAsync(ct) > 0);
        }
       
        // Nothing to change
        return OperationResult.Success();
    }
}