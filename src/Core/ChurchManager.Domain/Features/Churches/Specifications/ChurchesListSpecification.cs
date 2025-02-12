using Ardalis.Specification;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.Permissions.Services;
using ChurchManager.Domain.Shared;
using ChurchManager.Domain.Specifications;
using Microsoft.EntityFrameworkCore;
using CodeBoss.Extensions;

namespace ChurchManager.Domain.Features.Churches.Specifications;

public class ChurchesListSpecification: Specification<Church, ChurchViewModel>
{
    public ChurchesListSpecification(Guid userId, IPermissionService service, string searchTerm = null)
    {
        // Search Term
        if (!searchTerm.IsNullOrEmpty())
        {
            Query
                .Where(cg =>
                    // Name Search
                    EF.Functions.ILike(cg.Name, $"%{searchTerm}%") ||
                    EF.Functions.ILike(cg.Description, $"%{searchTerm}%"));
        }
        
        PermissionCriteria<Church>.AddPermissionCriteria(Query, userId, service);
        
        // Add ordering
        Query.OrderBy(x => x.Name);
        
        // View Model Mapping
        Query.Select(x => new ChurchViewModel
        {
            Id = x.Id,
            Name = x.Name,  
            Description = x.Description,
            ShortCode = x.ShortCode,
            LeaderPerson = x.LeaderPersonId.HasValue ? ToBasicPerson(x.LeaderPerson) : null
        });
    }
    
    private static Shared.PersonViewModelBasic ToBasicPerson(Person person)
    {
        return new Shared.PersonViewModelBasic
        {
            PersonId = person.Id,
            Gender = person.Gender,
            FirstName = person.FullName.FirstName,
            LastName = person.FullName.LastName,
            AgeClassification = person.AgeClassification,
            Age = person.BirthDate.Age,
            PhotoUrl = person.PhotoUrl
        };
    }
}