using Ardalis.Specification;
using ChurchManager.Domain.Specifications;
using CodeBoss.Extensions;

namespace ChurchManager.Domain.Features.People.Specifications;

public class FilterPeopleQuerySpecification: PermissionSpecification<Person>
{
   public FilterPeopleQuerySpecification(IList<int> personIds, IEnumerable<int>? allowedIds = null) : base(allowedIds)
   {
      Query.AsNoTracking();
      
      if (!personIds.IsNullOrEmpty())
      {
         Query.Where(x => personIds.Contains(x.Id));
      }
   }
}