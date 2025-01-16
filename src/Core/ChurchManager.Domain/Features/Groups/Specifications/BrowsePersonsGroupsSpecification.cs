using Ardalis.Specification;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Parameters;
using ChurchManager.Domain.Shared;
using CodeBoss.Extensions;

namespace ChurchManager.Domain.Features.Groups.Specifications
{
    public class BrowsePersonsGroupsSpecification : Specification<Group, PersonGroupsSummaryViewModel>
    {
        public BrowsePersonsGroupsSpecification(int personId, SearchTermQueryParameter query)
        {
            Query.AsNoTracking();

            // The groups this person is the leader of
            Query.Where(g =>
                g.Members
                    .Any(m => m.PersonId == personId && 
                              m.RecordStatus == RecordStatus.Active));

            // Filter the groups
            if(!string.IsNullOrEmpty(query.SearchTerm))
            {
                // Search By Name Or Description
                Query
                    .Search(x => x.Name, "%" + query.SearchTerm + "%")
                    .Search(x => x.Description, "%" + query.SearchTerm + "%")
                    ;
            }

            if(!query.OrderBy.IsNullOrEmpty())
            {
                if(query.SortOrder == "ascending" || query.SortOrder == "ASC")
                {
                    Query.OrderBy(query.OrderBy);
                }
                else
                {
                    Query.OrderByDescending(query.OrderBy);
                }
            }

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
                CanEdit = x.Members.First(m => m.PersonId == personId).GroupRole.CanEdit,
                CanView = x.Members.First(m => m.PersonId == personId).GroupRole.CanView,
                CanManageMembers = x.Members.First(m => m.PersonId == personId).GroupRole.CanManageMembers,
                MembersCount = x.Members.Count(m => m.RecordStatus == RecordStatus.Active)
            });
        }
    }
}
