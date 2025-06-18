using Ardalis.Specification;
using ChurchManager.Domain.Features.Churches.Extensions;
using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.People.Extensions;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.People.Specifications;

public class ProfileByUserLoginSpecification : Specification<Person, PersonViewModel>, ISingleResultSpecification<Person, PersonViewModel>
{
    public ProfileByUserLoginSpecification(string userLoginId)
    {
        Query
            .Where(x => x.UserLoginId == userLoginId)

            .Include(x => x.Church)
            .Include(x => x.PhoneNumbers)
            .Include(x => x.Family).ThenInclude(f => f.FamilyMembers);
        
        Query.Select(p => p.ToViewModel(true));
    }
}

public class ProfileByPersonSpecification : Specification<Person, PersonViewModel>, ISingleResultSpecification<Person, PersonViewModel>
{
    public ProfileByPersonSpecification(int personId, bool condensed = false)
    {
        Query
            .Where(x => x.Id == personId)

            .Include(x => x.Church)
            .Include(x => x.PhoneNumbers);

        if(!condensed)
        {
            Query.Include(x => x.Family).ThenInclude(f => f.FamilyMembers);
        }

        Query.Select(p => p.ToViewModel(condensed));
    }
}