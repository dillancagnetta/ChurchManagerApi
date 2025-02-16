using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Security;
using ChurchManager.Domain.Features.Security.Repositories;
using ChurchManager.Domain.Features.Security.Services;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Codeboss.Types;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.Auth.Services;


/*
 
 "DynamicScope" concept to EntityPermission. 
 This would allow us to define rules like "All churches in ChurchGroup X" or "All groups in Church Y" 
 and have it automatically include new entities.
 
 // Church Group Admin gets dynamic access to all churches in their group
var permission = new EntityPermission
{
    EntityType = "Church",
    IsDynamicScope = true,
    ScopeType = "ChurchGroup",
    ScopeId = churchGroupId,
    CanView = true,
    CanEdit = true
};
 
 */

public class PermissionService(
    IUserLoginRoleDbRepository rolesDb,
    IEntityPermissionDbRepository permissionsDb,
    ChurchManagerDbContext dbContext) : IPermissionService
{
    /// <summary>
    /// Checks both explicit IDs and dynamic scopes
    ///     For dynamic scopes, queries the relationships in real-time
    ///    Automatically includes any new entities that match the scope
    /// </summary>
    public async Task<bool> HasPermissionAsync<T>(Guid userLoginId, int entityId, PermissionAction permission,
        CancellationToken ct = default) where T : class, IEntity<int>
    {
        var entityType = typeof(T).Name;
        
        var permissions = await rolesDb
            .Queryable()
            .AsNoTracking()
                .Include(x => x.Permissions)
            .Where(ulr => ulr.UserLoginId == userLoginId && ulr.RecordStatus == RecordStatus.Active.Value)
            .SelectMany(ulr => ulr.Permissions)
                .Where(ep => ep.RecordStatus == RecordStatus.Active.Value)
                .Where(ep => ep.EntityType == entityType)
            .ToListAsync(ct);
        
        foreach (var ep in permissions)
        {
            if (!HasPermissionFlag(ep, permission)) continue;

            // Check explicit IDs first
            if (!ep.IsDynamicScope && ep.EntityIds != null)
            {
                if (ep.EntityIds.Contains(entityId)) return true;
                continue;
            }

            // Check dynamic scope
            if (ep.IsDynamicScope)
            {
                var hasAccess = await CheckDynamicScopeAccessAsync(ep, entityType, entityId);
                if (hasAccess) return true;
            }
        }

        return false;    }

    public async Task<IQueryable<T>> FilterByPermissionAsync<T>(Guid userLoginId, IQueryable<T> query, PermissionAction permission, CancellationToken ct = default) where T : class, IEntity<int>
    {
        var entityType = typeof(T).Name;
        
        var permissions = await rolesDb
            .Queryable()
            .AsNoTracking()
                .Include(x => x.Permissions)
            .Where(ulr => ulr.UserLoginId == userLoginId && ulr.RecordStatus == RecordStatus.Active.Value)
            .SelectMany(ulr => ulr.Permissions)
            .Where(ep => ep.EntityType == entityType)
            .ToListAsync(ct);

        var accessibleIds = new HashSet<int>();

        foreach (var ep in permissions)
        {
            if (!HasPermissionFlag(ep, permission)) continue;

            // Add explicit IDs
            if (!ep.IsDynamicScope && ep.EntityIds != null)
            {
                foreach (var id in ep.EntityIds)
                    accessibleIds.Add(id);
            }

            // Add dynamic scope IDs
            if (ep.IsDynamicScope)
            {
                var dynamicIds = await GetDynamicScopeIdsAsync(ep);
                foreach (var id in dynamicIds) accessibleIds.Add(id);
            }
        }

        return query.Where(e => accessibleIds.Contains(e.Id));
    }

    public async Task GrantPermissionAsync(int userLoginRoleId, string entityType, int[] entityIds, IEnumerable<PermissionAction> permissions,
        CancellationToken ct = default)
    {
        // Check if permission already exists for this role and entity type
        var existingPermission = await permissionsDb
            .Queryable()
                /*.Include(x => x.Permissions)
            //.Where(ulr => ulr.RecordStatus == RecordStatus.Active.Value)
            .SelectMany(ulr => ulr.Permissions)*/
            .FirstOrDefaultAsync(ep => 
                ep.UserLoginRoleId == userLoginRoleId && 
                ep.EntityType == entityType, cancellationToken: ct);

        if (existingPermission != null)
        {
            // Update existing permission
            existingPermission.EntityIds = entityIds;
            UpdatePermissionFlags(existingPermission, permissions);
        }
        else
        {
            // Create new permission
            var entityPermission = new EntityPermission
            {
                UserLoginRoleId = userLoginRoleId,
                EntityType = entityType,
                EntityIds = entityIds,
            };
            
            UpdatePermissionFlags(entityPermission, permissions);
            permissionsDb.AddAsync(entityPermission, ct);
        }

        await permissionsDb.SaveChangesAsync(ct);
    }

    public async Task RevokePermissionAsync(int userLoginRoleId, string entityType, CancellationToken ct = default)
    {
        var permission = await permissionsDb
            .Queryable()
            .FirstOrDefaultAsync(ep => 
                ep.UserLoginRoleId == userLoginRoleId && 
                ep.EntityType == entityType, ct);

        if (permission != null)
        {
            permissionsDb.DeleteAsync(permission, ct);
            await permissionsDb.SaveChangesAsync(ct);
        }    
    }

    public async Task<IReadOnlyList<int>> GetAllowedEntityIdsAsync<T>(Guid userLoginId, PermissionAction permission, CancellationToken ct = default) where T : class, IEntity<int>
    {
        if (await IsSystemAdminAsync(userLoginId, ct)) return null;
        
        var entityType = typeof(T).Name;
        
        var permissions = await rolesDb
            .Queryable()
            .AsNoTracking()
                .Include(x => x.Permissions)
            .Where(ulr => ulr.UserLoginId == userLoginId && ulr.RecordStatus == RecordStatus.Active.Value)
                .SelectMany(ulr => ulr.Permissions)
                .Where(ep => ep.EntityType == entityType)
            .ToListAsync(ct);
        
        var accessibleIds = new HashSet<int>();

        foreach (var ep in permissions)
        {
            if (!HasPermissionFlag(ep, permission)) continue;

            // Add explicit IDs
            if (!ep.IsDynamicScope && ep.EntityIds != null)
            {
                accessibleIds.UnionWith(ep.EntityIds);
            }

            // Add dynamic scope IDs
            if (ep.IsDynamicScope)
            {
                var dynamicIds = await GetDynamicScopeIdsAsync(ep);
                accessibleIds.UnionWith(dynamicIds);
            }
        }

        return accessibleIds.ToList();
    }

    public Task<bool> IsSystemAdminAsync(Guid userLoginId, CancellationToken ct = default)
    {
        return rolesDb.Queryable()
            .AsNoTracking()
            .AnyAsync(r => r.UserLoginId == userLoginId && r.Name == UserLoginRole.SystemAdminRoleName, ct);
    }

    private bool HasPermissionFlag(EntityPermission permission, string permissionType)
    {
        return permissionType switch
        {
            "View" => permission.CanView,
            "Edit" => permission.CanEdit,
            "Delete" => permission.CanDelete,
            "ManageUsers" => permission.CanManageUsers,
            _ => false
        };
    }
    
    private void UpdatePermissionFlags(EntityPermission permission, IEnumerable<PermissionAction> permissions)
    {
        foreach (var p in permissions)
        {
            switch (p.Value)
            {
                case "View":
                    permission.CanView = true;
                    break;
                case "Edit":
                    permission.CanEdit = true;
                    break;
                case "Delete":
                    permission.CanDelete = true;
                    break;
                case "ManageUsers":
                    permission.CanManageUsers = true;
                    break;
            }
        }
    }
    
    private async Task<bool> CheckDynamicScopeAccessAsync(
        EntityPermission permission,
        string entityType,
        int entityId)
    {
        switch (entityType)
        {
            case "Church" when permission.ScopeType == "ChurchGroup":
                return await dbContext.Church
                    .AnyAsync(c => c.Id == entityId && 
                                   c.ChurchGroupId == permission.ScopeId);

            case "Group" when permission.ScopeType == "ChurchGroup":
                return await dbContext.Group
                    .AnyAsync(g => g.Id == entityId && 
                                   g.Church.ChurchGroupId == permission.ScopeId);

            case "Group" when permission.ScopeType == "Church":
                return await dbContext.Group
                    .AnyAsync(g => g.Id == entityId && 
                                   g.ChurchId == permission.ScopeId);

            default:
                return false;
        }
    }
    
    private async Task<IReadOnlyList<int>> GetDynamicScopeIdsAsync(EntityPermission permission)
    {
        switch (permission.EntityType)
        {
            // Defines: Access to Churches in the ChurchGroup
            case "Church" when permission.ScopeType == "ChurchGroup":
                return await dbContext.Church
                    .Where(c => c.ChurchGroupId == permission.ScopeId)
                    .Select(c => c.Id)
                    .ToListAsync();

            // Defines: Access to Groups in the ChurchGroup
            case "Group" when permission.ScopeType == "ChurchGroup":
                return await dbContext.Group
                    .Where(g => g.Church.ChurchGroupId == permission.ScopeId)
                    .Select(g => g.Id)
                    .ToListAsync();

            // Defines: Access to Groups in the Church
            case "Group" when permission.ScopeType == "Church":
                return await dbContext.Group
                    .Where(g => g.ChurchId == permission.ScopeId)
                    .Select(g => g.Id)
                    .ToListAsync();
            
            // Defines: Access to People in the Church
            case "Person" when permission.ScopeType == "Church":
                return await dbContext.Person
                    .Where(g => g.ChurchId == permission.ScopeId)
                    .Select(g => g.Id)
                    .ToListAsync();
            
            // Defines: Access to Child Groups of Parent
            case "Group" when permission.ScopeType == "ParentGroup":
                var query = @"
                        WITH RecursiveGroups AS (
                            SELECT Id FROM Group WHERE Id = {0}
                            UNION ALL
                            SELECT g.Id FROM Group g
                                INNER JOIN RecursiveGroups rg ON g.ParentId = rg.Id
                        )
                        SELECT Id FROM RecursiveGroups;";
                return await dbContext.Group.FromSqlRaw(query, permission.ScopeId) // ScopeId is the Parent Group Id
                    .Select(g => g.Id)
                    .ToListAsync();

            default:
                return Array.Empty<int>(); // No access
        }
    }
}