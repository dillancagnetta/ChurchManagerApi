using Ardalis.Specification;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Groups.Specifications
{
    public class PersonsGroupsSpecification : Specification<Group, PersonGroupsSummaryViewModel>
    {
        public PersonsGroupsSpecification(int personId, RecordStatus recordStatus)
        {
            Query.AsNoTracking();

            Query.Where(x =>
                x.Members
                    .Any(x =>
                        x.PersonId == personId &&
                        //x.GroupRole.IsLeader &&
                        x.RecordStatus == recordStatus));

            Query.Include(x => x.GroupType);
            Query.Include(x => x.Members)
                .ThenInclude(x => x.GroupRole);

            Query.Select(x => new PersonGroupsSummaryViewModel
            {
                GroupId = x.Id,
                Name = x.Name,
                Description = x.Description,
                ParentGroupId = x.ParentGroupId,
                GroupType = x.GroupType.Name,
                GroupRole = x.Members.First(m => m.PersonId == personId).GroupRole.Name,
                RecordStatus = x.RecordStatus.ToString(),
                TakesAttendance = x.GroupType.TakesAttendance,
                IsLeader = x.Members.First(m => m.PersonId == personId).GroupRole.IsLeader,
                MembersCount = x.Members.Count(m => m.RecordStatus == recordStatus)
            });
        }  
    }
}
