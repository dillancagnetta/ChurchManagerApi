using Ardalis.Specification;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Shared;
using CodeBoss.Extensions;
using Convey.CQRS.Queries;

namespace ChurchManager.Domain.Features.Discipleship.Specifications
{
    public class BrowseDiscipleshipParticipants : Specification<DiscipleshipStep, DiscipleshipStepViewModel>
    {
        public BrowseDiscipleshipParticipants(
            IPagedQuery paging,
            int stepDefinitionId, string status, DateTime? from, DateTime? to)
        {
            Query.AsNoTracking();
            Query.Include(x => x.Person);

            Query.Where(g => g.Id == stepDefinitionId);

            // Status Filder
            if (!status.IsNullOrEmpty())
            {
                Query.Where(g => g.Status == status);
            }

            // Date Filters
            if(from.HasValue)
            {
                Query.Where(g => g.CreatedDate >= from.Value);
            }
            if(to.HasValue)
            {
                Query.Where(g => g.CreatedDate <= to.Value);
            }

            Query
                .Skip(paging.CalculateSkip())
                .Take(paging.CalculateTake());

            Query.Select(step => new DiscipleshipStepViewModel
                {
                    Person = new PersonViewModelBasic
                    {
                        PersonId = step.PersonId,
                        FirstName = step.Person!.FullName!.FirstName!,
                        LastName = step.Person.FullName.LastName!,
                        Gender = step.Person.Gender,
                        AgeClassification = step.Person.AgeClassification,
                        PhotoUrl = step.Person.PhotoUrl,
                        Age = step.Person.BirthDate != null ? step.Person.BirthDate.Age : null,
                    },
                    CompletionDate = step.CompletionDate,
                    Status = step.Status,
                    IsComplete = step.IsComplete,
                    /*StepDefinition = new StepDefinitionViewModel
                    {
                        Order = step.Definition.Order,
                        Id = step.Definition.Id,
                        Description = step.Definition.Description,
                        Name = step.Definition.Name,
                    }*/
                })
                .OrderBy(step => step.Status);
        }
    }
}
