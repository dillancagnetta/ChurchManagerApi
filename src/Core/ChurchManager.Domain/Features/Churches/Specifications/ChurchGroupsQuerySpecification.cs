using Ardalis.Specification;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Shared;
using CodeBoss.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Domain.Features.Churches.Specifications;

public class ChurchGroupsQuerySpecification: Specification<ChurchGroup, ChurchGroupViewModel>
{
    public ChurchGroupsQuerySpecification(string searchTerm)
    {
        Query.Include(cg => cg.Churches).ThenInclude(c => c.LeaderPerson);
        Query.Include(cg => cg.LeaderPerson);
        
        // Search Term
        if (!searchTerm.IsNullOrEmpty())
        {
            Query
                .Where(cg =>
                    // Name Search
                    EF.Functions.ILike(cg.Name, $"%{searchTerm}%") ||
                    EF.Functions.ILike(cg.Description, $"%{searchTerm}%"));
        }
        
        Query.Select(x => new ChurchGroupViewModel
        {
            Id = x.Id,
            Name = x.Name,  
            Description = x.Description,
            LeaderPerson = x.LeaderPersonId.HasValue ? ToAutoCompletePerson(x.LeaderPerson) : null,
            Churches = x.Churches.Select(c => new ChurchViewModel
            { 
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ShortCode = c.ShortCode,
                LeaderPerson = c.LeaderPersonId.HasValue ? ToAutoCompletePerson(c.LeaderPerson) : null,
            })
        });
    }
    
    private static PeopleAutocompleteViewModel ToAutoCompletePerson(Person person)
    {
        return new PeopleAutocompleteViewModel(
            person.Id,
            $"{person.FullName.FirstName} {person.FullName.LastName}",
            person.PhotoUrl,
            person.ConnectionStatus);
    }
}