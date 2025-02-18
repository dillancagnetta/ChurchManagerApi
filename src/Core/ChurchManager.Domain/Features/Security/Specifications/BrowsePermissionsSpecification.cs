using Ardalis.Specification;
using ChurchManager.Domain.Shared;
using CodeBoss.Extensions;

namespace ChurchManager.Domain.Features.Security.Specifications;

public class BrowsePermissionSpecification : Specification<EntityPermission, PermissionViewModel>
{
    public BrowsePermissionSpecification(string searchTerm, int? entityId = null, bool? isDynamicScope = null)
    {
        Query.AsNoTracking();

        if (!searchTerm.IsNullOrEmpty())
        {
            Query.Search(x => x.EntityType, searchTerm);
            Query.Search(x => x.ScopeType, searchTerm);
        }
        
        if (entityId.HasValue)
        {
            Query.Where(x => x.EntityIds.Contains(entityId.Value));
        }
        
        if (isDynamicScope.HasValue)
        {
            Query.Where(x => x.IsDynamicScope == isDynamicScope.Value);
        }
        
        Query.Select(x => new PermissionViewModel
        {
            Id = x.Id,
            RecordStatus = x.RecordStatus.ToString(),
            EntityType = x.EntityType,
            ScopeType = x.ScopeType,
            IsSystem = x.IsSystem,
            EntityIds = x.EntityIds,
            IsDynamicScope = x.IsDynamicScope,
            CanView = x.CanView,
            CanEdit = x.CanEdit,
            CanDelete = x.CanDelete,
            CanManageUsers = x.CanManageUsers
        });
    }
}