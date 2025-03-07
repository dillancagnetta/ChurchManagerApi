using Ardalis.Specification;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Shared;
using CodeBoss.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Domain.Features.Churches.Specifications;

public class ChurchesListSpecification: Specification<Church, ChurchViewModel>
{
    public ChurchesListSpecification(IEnumerable<int> allowedChurchIds = null, string searchTerm = null)
    {
        // Only apply permission filter if allowedIds is not null
        // If null, user is system admin and has unrestricted access
        if (allowedChurchIds is not null)
        {
            // First apply the permissions filter
            Query.Where(x => allowedChurchIds.Contains(x.Id));  
        }
        
        // Search Term
        if (!searchTerm.IsNullOrEmpty())
        {
            Query
                .Where(cg =>
                    // Name Search
                    EF.Functions.ILike(cg.Name, $"%{searchTerm}%") ||
                    EF.Functions.ILike(cg.Description, $"%{searchTerm}%"));
        }
        
        // PermissionCriteria<Church>.AddPermissionCriteria(Query, userId, service);
        
        // Add ordering
        Query.OrderBy(x => x.Name);
        
        // View Model Mapping
        Query.Select(x => new ChurchViewModel
        {
            Id = x.Id,
            Name = x.Name,  
            Description = x.Description,
            ShortCode = x.ShortCode,
            LeaderPerson = x.LeaderPersonId.HasValue ? Person.ToBasicPerson(x.LeaderPerson) : null
        });
    }
}