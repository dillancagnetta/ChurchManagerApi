using ChurchManager.Features.Groups.Commands.AddGroupMember;
using ChurchManager.Features.Groups.Commands.GroupAttendanceRecord;
using ChurchManager.Features.Groups.Commands.NewGroup;
using ChurchManager.Features.Groups.Commands.RemoveGroupMember;
using ChurchManager.Features.Groups.Queries.BrowsePersonsGroups;
using ChurchManager.Features.Groups.Queries.GroupMembers;
using ChurchManager.Features.Groups.Queries.GroupPerformanceMetrics;
using ChurchManager.Features.Groups.Queries.GroupRoles;
using ChurchManager.Features.Groups.Queries.GroupsForChurch;
using ChurchManager.Features.Groups.Queries.GroupsForPerson;
using ChurchManager.Features.Groups.Queries.GroupsWithChildren;
using ChurchManager.Features.Groups.Queries.GroupTypes;
using ChurchManager.Features.Groups.Queries.GrroupsByGroupType;
using ChurchManager.SharedKernel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class GroupTypesController : BaseApiController
    {
        private readonly ICognitoCurrentUser _currentUser;

        public GroupTypesController(ICognitoCurrentUser currentUser)
        {
            _currentUser = currentUser;
        }
       
        [HttpPost("browse")]
        public async Task<IActionResult> GetGroupTypes([FromBody] GetGroupTypesQuery query, CancellationToken token)
        {
            return Ok(await Mediator.Send(query, token));
        }
        


       
    }
}
