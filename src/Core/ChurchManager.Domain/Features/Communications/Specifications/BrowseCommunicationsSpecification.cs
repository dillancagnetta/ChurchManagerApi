using Ardalis.Specification;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Shared;
using ChurchManager.Domain.Specifications;
using CodeBoss.Extensions;
using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;
using PersonViewModel = ChurchManager.Domain.Shared.PersonViewModelBasic;

namespace ChurchManager.Domain.Features.Communications.Specifications;

public class BrowseCommunicationsSpecification: PermissionSpecification<Communication, CommunicationViewModel>
{
    public BrowseCommunicationsSpecification(
        IPagedQuery paging,
        IEnumerable<string>? types  = null,
        string? status  = null,
        string? searchTerm = null, 
        DateTime? from  = null,  DateTime? to  = null, 
        int? recipientPersonId  = null, 
        int? communicationTemplateId  = null,
        int? churchGroupId  = null,
        int? churchId = null,
        int? listGroupId  = null,
        IEnumerable<int>? allowedIds = null)
        : base(allowedIds)
    {
        Query.Include(cg => cg.ListGroup);
        Query.Include(cg => cg.SenderPerson);
        Query.Include(cg => cg.Recipients);
        Query.Include(cg => cg.CommunicationTemplate);
        
        // Search Term
        if (!searchTerm.IsNullOrEmpty())
        {
            Query
                .Where(cg =>
                    // Name Search
                    EF.Functions.ILike(cg.Name, $"%{searchTerm}%") ||
                    EF.Functions.ILike(cg.Subject, $"%{searchTerm}%"));
        }
        
        // Date Filters
        if(from.HasValue)
        {
            Query.Where(g =>g.SendDateTime.HasValue && g.SendDateTime >= from.Value);
        }
        if(to.HasValue)
        {
            Query.Where(g =>g.SendDateTime.HasValue && g.SendDateTime <= to.Value);
        }

        if (recipientPersonId.HasValue)
        {
            Query.Where(g => g.Recipients.Any(r => r.PersonId == recipientPersonId.Value));
        }
        
        if (communicationTemplateId.HasValue)
        {
            Query.Where(g => g.CommunicationTemplateId == communicationTemplateId);
        }
        
        if (!types.IsNullOrEmpty())
        {
            Query.Where(g => types.ToList().Contains(g.CommunicationType));
        }
        
        if (status != null)
        {
            Query.Where(g => g.Status == status);
        }
        
        Query.OrderBy(x => x.Name);
        
        Query
            .Skip(paging.CalculateSkip())
            .Take(paging.CalculateTake());
        
        Query.Select(x => new CommunicationViewModel
        {
            Id = x.Id,
            Name = x.Name!,  
            Subject = x.Subject,
            Content = x.CommunicationContent!,
            Category = x.Category,
            SenderPerson = x.SenderPersonId.HasValue ? ToBasicPerson(x.SenderPerson!) : null,
            ListGroup = x.ListGroupId.HasValue ? new GroupReference { GroupId = x.ListGroup!.Id, GroupName = x.ListGroup.Name } : null,
            CommunicationType = x.CommunicationType,
            CommunicationTemplateId = x.CommunicationTemplateId,
            CommunicationContent = x.CommunicationContent,
            IsBulkCommunication = x.IsBulkCommunication,
            SendDateTime = x.SendDateTime,
            CreatedDateTime = x.CreatedDate,
            Status = x.Status.Value,
            RecipientCount = x.Recipients.Count,
            SystemCommunicationId = x.SystemCommunicationId,
            Metadata = x.Metadata,
            Review = x.Review != null ? new CommunicationReviewViewModel
            {
                ReviewerNote = x.Review.ReviewerNote,
                ReviewedDateTime = x.Review.ReviewedDateTime,
                ReviewerPersonId = x.Review.ReviewerPersonId
            } : null
        });
    }
    
    private static PersonViewModel ToBasicPerson(Person person)
    {
        return new PersonViewModel
        {
            PersonId = person.Id,
            Gender = person.Gender,
            FirstName = person.FullName!.FirstName!,
            LastName = person.FullName!.LastName!,
            AgeClassification = person.AgeClassification,
            Age = person.BirthDate?.Age,
            PhotoUrl = person.PhotoUrl
        };
    }
}