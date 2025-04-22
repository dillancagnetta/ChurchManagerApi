using Ardalis.Specification;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.People.Specifications
{
    public class ConnectionStatusSelectSpecification : Specification<ConnectionStatusType, SelectItemViewModel>
    {
        public ConnectionStatusSelectSpecification()
        {
            Query.AsNoTracking();
           
            Query.Select(x => new SelectItemViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            });
        }
    }
}
