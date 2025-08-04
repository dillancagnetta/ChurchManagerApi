using Ardalis.Specification;
using ChurchManager.Domain.Features.People.Extensions;
using ChurchManager.Domain.Shared;
using ChurchManager.Domain.Specifications;
using CodeBoss.Extensions;

namespace ChurchManager.Domain.Features.People.Specifications;

public class FilterPeopleQuerySpecification: PermissionSpecification<Person, PersonViewModel>
{
   public FilterPeopleQuerySpecification(IList<int> personIds, IEnumerable<int>? allowedIds = null) : base(allowedIds)
   {
      Query.AsNoTracking();
      
      if (!personIds.IsNullOrEmpty())
      {
         Query.Where(x => personIds.Contains(x.Id));
      }
      
      Query.Select(p => p.ToViewModel(true)); // not condensed
   }
}