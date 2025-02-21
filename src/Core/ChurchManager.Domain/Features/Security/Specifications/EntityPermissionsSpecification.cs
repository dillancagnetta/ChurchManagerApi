using Ardalis.Specification;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Shared;
using CodeBoss.Extensions;

namespace ChurchManager.Domain.Features.Security.Specifications;

public class EntityPermissionsSpecification : Specification<EntityPermission, PermissionViewModel>
{
    public EntityPermissionsSpecification(IEnumerable<int> excludeIds = null)
    {
        Query
            .AsNoTracking()
            .EnableCache(nameof(EntityPermissionsSpecification), excludeIds.ToCacheKey());

        if (!excludeIds.IsNullOrEmpty())
        {
            Query.Where(x => !excludeIds.Contains(x.Id));  
        }
        
        Query.Where(x => x.RecordStatus == RecordStatus.Active.Value);

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
            RecordStatus = x.RecordStatus.ToString()
        });

    }
}