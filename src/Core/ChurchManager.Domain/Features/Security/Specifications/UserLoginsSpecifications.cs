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
            .Include(x => x.Roles)
             .ThenInclude(r => r.Permissions);

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
            Roles = x.Roles.Select(r => new UserLoginRoleViewModel
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                IsSystem = r.IsSystem,
                RecordStatus = r.RecordStatus.ToString(),
                Permissions = r.Permissions.Select(p => new PermissionViewModel
                {
                    Id = p.Id,
                    EntityType = p.EntityType,
                    ScopeType = p.ScopeType,
                    IsSystem = p.IsSystem,
                    RecordStatus = p.RecordStatus.ToString(),
                    EntityIds = p.EntityIds,
                    CanView = p.CanView,
                    CanEdit = p.CanEdit,
                    CanDelete = p.CanDelete,
                    CanManageUsers = p.CanManageUsers
                }).ToList()
            } ).ToList()
        });
    }
}