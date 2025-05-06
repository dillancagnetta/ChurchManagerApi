using Ardalis.Specification;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Shared;
using CodeBoss.Extensions;
using Convey.CQRS.Queries;

namespace ChurchManager.Domain.Features.Churches.Specifications;

public class BrowseRecipientsSpecification : Specification<Communication, CommunicationRecipientViewModel>
{
   public BrowseRecipientsSpecification(IPagedQuery paging, int communicationId, string? status = null, int? personId = null)
   {
      Query.AsNoTracking();
      Query.Include(x => x.Recipients);
      
      // Communication Filter
      Query.Where(g => g.Id == communicationId);
      
      // Person Filter
      if(personId.HasValue)
      {
         Query.Where(g => g.Recipients.Any(r => r.PersonId == personId));
      }
      
      // Status Filter
      if (!status.IsNullOrEmpty())
      {
         Query.Where(g => g.Recipients.Any(r => r.Status == status));
      }

      Query.OrderBy(x => x.SendDateTime);
      
      Query
         .Skip(paging.CalculateSkip())
         .Take(paging.CalculateTake());

      Query.SelectMany(c => c.Recipients.Select(x => new CommunicationRecipientViewModel
      {
         RecipientPerson = new PersonViewModelBasic
         {
            PersonId = x.PersonId,
            FirstName = x.RecipientPerson!.FullName!.FirstName!,
            LastName = x.RecipientPerson!.FullName!.LastName!,
            Gender = x.RecipientPerson.Gender,
            AgeClassification = x.RecipientPerson.AgeClassification,
            PhotoUrl = x.RecipientPerson.PhotoUrl,
         },
         Status = x.Status.ToString(),
         StatusNote = x.StatusNote,
         SendDateTime = x.SendDateTime,
         OpenedDateTime = x.OpenedDateTime,
         AttemptCount = x.AttemptCount
      }));
   }
}