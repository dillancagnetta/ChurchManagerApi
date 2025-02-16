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

        Query.Include(x => x.Permissions);
            
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
            UserLoginId = x.UserLoginId,
            RecordStatus = x.RecordStatus.ToString()
        });
    }
}