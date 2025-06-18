using Ardalis.Specification;
using ChurchManager.Domain.Features.People.Extensions;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.People.Specifications
{
    public class FamilyWithMembersSpecification : Specification<Family, FamilyViewModel>
    {
        public FamilyWithMembersSpecification(int familyId, bool includePeople)
        {
            
            Query
                .Where(x => x.Id == familyId)
                .AsNoTracking()
                ;

            if (includePeople)
            {
                Query.Include(x => x.FamilyMembers);
                Query.Include(x => x.Address);
            }
            
            Query.Select(x => new FamilyViewModel
            {
                Id = x.Id,
                Name = x.Name!,
                City = x.Address!.City,
                Country = x.Address.Country,
                PostalCode = x.Address.PostalCode,
                Street = x.Address.Street,
                Province = x.Address.Province,
                Language = x.Language,
                FamilyMembers = x.FamilyMembers.Select(x => x.ToBasicPersonViewModel())
            });
            
        }
    }
}