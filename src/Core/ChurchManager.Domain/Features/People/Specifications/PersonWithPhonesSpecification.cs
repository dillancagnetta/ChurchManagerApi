using Ardalis.Specification;

namespace ChurchManager.Domain.Features.People.Specifications
{
    public class PersonWithPhonesSpecification : Specification<Person>, ISingleResultSpecification<Person>
    {
        public PersonWithPhonesSpecification(int personId)
        {
            Query
                .Where(x => x.Id == personId)
                .Include(x => x.PhoneNumbers);
        }
    }
}
