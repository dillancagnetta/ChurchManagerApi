using Ardalis.Specification;

namespace ChurchManager.Domain.Features.Groups.Specifications
{
    public class GroupTypeForGroupSpecification : Specification<Group, GroupType>, ISingleResultSpecification<Group>
    {
        public GroupTypeForGroupSpecification(int groupId)
        {
            Query.AsNoTracking();

            Query.Where(group => group.Id == groupId);

            Query.Select(x => x.GroupType);
        }
    }
}
