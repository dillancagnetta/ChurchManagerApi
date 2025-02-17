using Ardalis.Specification;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Shared;
using ChurchManager.Domain.Specifications;
using CodeBoss.Extensions;

namespace ChurchManager.Domain.Features.Security.Specifications;

public class UserLoginRolesSpecification : PermissionSpecification<UserLoginRole, UserLoginRoleViewModel>
{
    public UserLoginRolesSpecification(string searchTerm, IEnumerable<int> allowedChurchIds = null)
        : base(allowedChurchIds)
    {
        Query.AsNoTracking();

        Query
            .Include(x => x.PermissionAssignments)
            .ThenInclude(pa => pa.Permission)
            .Include(x => x.UserAssignments)
            .ThenInclude(ua => ua.UserLogin);
            
        if (!searchTerm.IsNullOrEmpty())
        {
            Query.Search(x => x.Name, searchTerm);
            Query.Search(x => x.Description, searchTerm);
        }
        
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
                IsSystem = pa.Permission.IsSystem,
                RecordStatus = pa.Permission.RecordStatus.ToString(),
                EntityIds = pa.Permission.EntityIds,
                CanView = pa.Permission.CanView,
                CanEdit = pa.Permission.CanEdit,
                CanDelete = pa.Permission.CanDelete,
                CanManageUsers = pa.Permission.CanManageUsers
            }).ToList()
        });
    }
}