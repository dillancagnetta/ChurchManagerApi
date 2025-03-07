using Ardalis.Specification;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Shared;
using CodeBoss.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Domain.Features.Security.Specifications;

public class EntityPermissionsSpecification : Specification<EntityPermission, PermissionViewModel>
{
    public EntityPermissionsSpecification(IEnumerable<int> excludeIds = null, int? UserLoginRoleId = null)
    {
        Query
            .AsNoTracking()
            .EnableCache(nameof(EntityPermissionsSpecification), 
                condition: !excludeIds.IsNullOrEmpty() && !UserLoginRoleId.HasValue,
                excludeIds.ToCacheKey());

        if (!excludeIds.IsNullOrEmpty())
        {
            Query.Where(x => !excludeIds.Contains(x.Id));  
        }

        if (UserLoginRoleId.HasValue)
        {
            Query.Include(x => x.RoleAssignments);
            Query.Where(x => x.RoleAssignments.Any(ra => ra.RoleId == UserLoginRoleId.Value));
        }
        
        Query.Where(x => x.RecordStatus == RecordStatus.Active.Value);

        Query.OrderBy(x => x.EntityType)
            .ThenBy(x => x.IsDynamicScope)
            .ThenBy(x => x.ScopeType)
            .ThenBy(x => x.ScopeId)
            .ThenBy(x => x.CanView)
            .ThenBy(x => x.CanEdit)
            .ThenBy(x => x.CanDelete)
            ;

        Query.Select(x => new PermissionViewModel
        {
            Id = x.Id,
            EntityType = x.EntityType,
            ScopeType = x.ScopeType,
            ScopeId = x.ScopeId,
            EntityIds = x.EntityIds,
            IsDynamicScope = x.IsDynamicScope,
            CanView = x.CanView,
            CanEdit = x.CanEdit,
            CanDelete = x.CanDelete,
            CanManageUsers = x.CanManageUsers,
            RecordStatus = x.RecordStatus.ToString(),
            IsSystem = x.IsSystem,
        });

    }
}