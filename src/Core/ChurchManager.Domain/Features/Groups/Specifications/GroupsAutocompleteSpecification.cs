using Ardalis.Specification;
using ChurchManager.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Domain.Features.Groups.Specifications
{
    public class GroupsAutocompleteSpecification : Specification<Group, GroupsAutocompleteViewModel>
    {
        public GroupsAutocompleteSpecification(string searchTerm)
        {
            Query.AsNoTracking();
            
            // Any match will do here
            Query.Where(x =>
                EF.Functions.ILike(x.Name, $"%{searchTerm}%") ||
                EF.Functions.ILike(x.Description, $"%{searchTerm}%")
            );
            
            Query.Include(x => x.GroupType);

            Query.Select(x => new GroupsAutocompleteViewModel(x.Id, x.Name, x.GroupType.Name));
        }
    }
}
