using Ardalis.Specification;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Shared;
using ChurchManager.Domain.Specifications;
using CodeBoss.Extensions;

namespace ChurchManager.Domain.Features.Security.Specifications;

public class UserLoginRolesSpecification : PermissionSpecification<UserLoginRole, UserLoginRoleViewModel>
{
    public UserLoginRolesSpecification(
        string searchTerm, IEnumerable<int> excludeIds = null, IEnumerable<int> allowedIds = null,  Guid? userLoginId = null)
        : base(allowedIds)
    {
        Query
            .AsNoTracking()
            .EnableCache(nameof(UserLoginRolesSpecification), searchTerm, excludeIds.ToCacheKey(), allowedIds.ToCacheKey());

        Query
            .Include(x => x.PermissionAssignments)
            .ThenInclude(pa => pa.Permission);
            /*.Include(x => x.UserAssignments)
            .ThenInclude(ua => ua.UserLogin);*/
            
        if (!searchTerm.IsNullOrEmpty())
        {
            Query.Search(x => x.Name, searchTerm);
            Query.Search(x => x.Description, searchTerm);
        }

        if (!excludeIds.IsNullOrEmpty())
        {
            Query.Where(x => !excludeIds.Contains(x.Id));  
        }
        
        if (userLoginId.HasValue)
        {
            Query.Include(x => x.UserAssignments.Where(ra => ra.UserLoginId == userLoginId.Value))
                .ThenInclude(ua => ua.UserLogin);
        }
        else
        {
            Query.Include(x => x.UserAssignments)
                .ThenInclude(ua => ua.UserLogin);
        }
        
        Query.Where(x => x.RecordStatus == RecordStatus.Active.Value);
        
        Query.Select(x => new UserLoginRoleViewModel
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            IsSystem = x.IsSystem,
            RecordStatus = x.RecordStatus.ToString(),
            UserLogins = x.UserAssignments.Select(ua => new UserLoginBasicViewModel
            {
                Id = ua.UserLoginId,
                Username = ua.UserLogin.Username
            }).ToList(),
            Permissions = x.PermissionAssignments.Select(pa => new PermissionViewModel
            {
                Id = pa.Permission.Id,
                EntityType = pa.Permission.EntityType,
                ScopeType = pa.Permission.ScopeType,
                ScopeId = pa.Permission.ScopeId,
                IsSystem = pa.Permission.IsSystem,
                RecordStatus = pa.Permission.RecordStatus.ToString(),
                IsDynamicScope = pa.Permission.IsDynamicScope,
                EntityIds = pa.Permission.EntityIds,
                CanView = pa.Permission.CanView,
                CanEdit = pa.Permission.CanEdit,
                CanDelete = pa.Permission.CanDelete,
                CanManageUsers = pa.Permission.CanManageUsers
            }).ToList()
        });
    }
}