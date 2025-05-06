using Ardalis.Specification;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Specifications;
using ChurchManager.Domain.Shared;
using CodeBoss.Extensions;
using Microsoft.EntityFrameworkCore;
using PersonViewModel = ChurchManager.Domain.Shared.PersonViewModelBasic;

namespace ChurchManager.Domain.Features.Churches.Specifications;

public class ChurchGroupsQuerySpecification: PermissionSpecification<ChurchGroup, ChurchGroupViewModel>
{
    public ChurchGroupsQuerySpecification(string? searchTerm = null, bool IncludeDetails = true, IEnumerable<int>? allowedIds = null)
        : base(allowedIds)
    {
        if (IncludeDetails)
        {
            Query.Include(cg => cg.Churches)
                .ThenInclude(c => c.LeaderPerson);
            
            Query.Include(cg => cg.Churches)
                    .ThenInclude(c => c.ServiceTimes)
                    .ThenInclude(s => s.ChurchAttendanceType);
            
            Query.Include(cg => cg.LeaderPerson);
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

        Query.OrderBy(x => x.Name);
        
        Query.Select(x => new ChurchGroupViewModel
        {
            Id = x.Id,
            Name = x.Name,  
            Description = x.Description,
            LeaderPerson = IncludeDetails && x.LeaderPersonId.HasValue ? ToBasicPerson(x.LeaderPerson) : null,
            Churches = IncludeDetails 
                ? x.Churches.OrderBy(c => c.Name)
                    .Select(c => new ChurchViewModel
                    { 
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        ShortCode = c.ShortCode,
                        PhoneNumber = c.PhoneNumber,
                        Address = c.Address,
                        LeaderPerson = c.LeaderPersonId.HasValue ? ToBasicPerson(c.LeaderPerson) : null,
                        ServiceTimes = c.ServiceTimes.Select(s => new ChurchServiceTimeViewModel
                        {
                            Id = s.Id,
                            ChurchAttendanceTypeId = s.ChurchAttendanceTypeId,
                            ChurchAttendanceType = s.ChurchAttendanceType!.Name,
                            DayOfWeek = s.DayOfWeek,
                            Time = s.Time,
                        })
                    }).ToList()
                : null
        });
    }
    
    private static PersonViewModel? ToBasicPerson(Person? person)
    {
        if (person == null) return null;
        return new PersonViewModel
        {
            PersonId = person.Id,
            Gender = person.Gender,
            FirstName = person.FullName!.FirstName!,
            LastName = person.FullName.LastName!,
            AgeClassification = person.AgeClassification,
            Age = person.BirthDate?.Age,
            PhotoUrl = person.PhotoUrl
        };
    }
}