using Ardalis.Specification;
using ChurchManager.Domain.Shared;
using CodeBoss.Extensions;

namespace ChurchManager.Domain.Features.Security.Specifications;

public class BrowsePermissionSpecification : Specification<EntityPermission, PermissionViewModel>
{
    public BrowsePermissionSpecification(string entityType, string scopeType, int? entityId = null, bool? isDynamicScope = null)
    {
        Query.AsNoTracking();

        if (!entityType.IsNullOrEmpty())
        {
            Query.Search(x => x.EntityType, entityType.Replace(" ", ""));
        }
        
        if (!scopeType.IsNullOrEmpty())
        {
            Query.Search(x => x.ScopeType, scopeType.Replace(" ", ""));
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
            ScopeId = x.ScopeId,
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