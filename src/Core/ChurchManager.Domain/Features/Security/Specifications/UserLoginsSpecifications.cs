using Ardalis.Specification;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Shared;
using ChurchManager.Domain.Specifications;
using CodeBoss.Extensions;

namespace ChurchManager.Domain.Features.Security.Specifications;

public class UserLoginsSpecification : PermissionSpecification<UserLogin, UserLoginViewModel>
{
    public UserLoginsSpecification(string searchTerm, IEnumerable<int> allowedChurchIds = null)
        : base(allowedChurchIds)
    {
        Query.AsNoTracking();

        Query
            .Include(x => x.Person)
            .Include(x => x.UserRoles)
                .ThenInclude(ur => ur.Role)
                 .ThenInclude(r => r.PermissionAssignments)
                    .ThenInclude(rp => rp.Permission);

        if (!searchTerm.IsNullOrEmpty())
        {
            Query.Search(x => x.Username, searchTerm);
        }
        
        Query.Select(x => new UserLoginViewModel
        {
            Id = x.Id,
            Username = x.Username,
            RecordStatus = x.RecordStatus.ToString(),
            Person = Person.ToBasicPerson(x.Person),
            Roles = x.UserRoles.Select(ur => new UserLoginRoleViewModel
            {
                Id = ur.Role.Id,
                Name = ur.Role.Name,
                Description = ur.Role.Description,
                IsSystem = ur.Role.IsSystem,
                RecordStatus = ur.Role.RecordStatus.ToString(),
                Permissions = ur.Role.PermissionAssignments.Select(pa => new PermissionViewModel
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
            }).ToList()
        });
    }
}