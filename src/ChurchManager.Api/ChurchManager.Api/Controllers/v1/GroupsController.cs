﻿using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Common;
using ChurchManager.Application.Features.Groups.Commands.AddGroupMember;
using ChurchManager.Application.Features.Groups.Commands.GroupAttendanceRecord;
using ChurchManager.Application.Features.Groups.Queries.BrowsePersonsGroups;
using ChurchManager.Application.Features.Groups.Queries.GroupMembers;
using ChurchManager.Application.Features.Groups.Queries.GroupRoles;
using ChurchManager.Application.Features.Groups.Queries.GroupsForChurch;
using ChurchManager.Application.Features.Groups.Queries.GroupsForPerson;
using ChurchManager.Application.Features.Groups.Queries.GroupsWithChildren;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class GroupsController : BaseApiController
    {
        private readonly ICognitoCurrentUser _currentUser;

        public GroupsController(ICognitoCurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        [HttpGet("person/{personId}")]
        public async Task<IActionResult> GetAllPersonsGroups(int personId, CancellationToken token)
        {
            var groups = await Mediator.Send(new GroupsForPersonQuery(personId), token);
            return Ok(groups);
        }

        #region Browse
        [HttpPost("browse/person/{personId}")]
        public async Task<IActionResult> BrowseGroupsForPerson(int personId, [FromBody] BrowsePersonsGroupsQuery query, CancellationToken token)
        {
            query.PersonId = personId; // Reset to the person we are searching for in the URL
            return Ok(await Mediator.Send(query, token));
        }

        [HttpPost("browse/current-user")]
        public async Task<IActionResult> BrowseCurrentUserGroups([FromBody] BrowsePersonsGroupsQuery query, CancellationToken token)
        {
            query.PersonId = _currentUser.PersonId; // Reset to current person
            return Ok(await Mediator.Send(query, token));
        }
        #endregion

        // http://localhost/Groups/1/members?recordStatus=Pending
        [HttpGet("{groupId}/members")]
        public async Task<IActionResult> GetGroupMembers(int groupId, [FromQuery] string recordStatus = "Active", CancellationToken token = default)
        {
            return Ok(await Mediator.Send(new GroupMembersQuery(groupId){RecordStatus = recordStatus }, token));
        }

        [HttpGet("church/{churchId}")]
        public async Task<IActionResult> GetGroupsForChurchSelectItem(int churchId, CancellationToken token)
        {
            return Ok(await Mediator.Send(new GroupsForChurchSelectItemQuery(churchId), token));
        }

        [HttpGet("{groupTypeId}/grouproles")]
        public async Task<IActionResult> GetGroupRolesForGroupType(int groupTypeId, CancellationToken token)
        {
            return Ok(await Mediator.Send(new GroupRolesQuery(groupTypeId), token));
        }

        [HttpPost("{groupId}/attendance")]
        public async Task<IActionResult> PostGroupAttendanceRecord([FromBody] GroupAttendanceRecordCommand command,
            CancellationToken token)
        {
            await Mediator.Send(command, token);
            return Accepted();
        }

        [HttpGet("tree")]
        public async Task<IActionResult> GetGroupsWithChildrenTree(CancellationToken token)
        {
            return Ok(await Mediator.Send(new GroupsWithChildrenQuery(), token));
        }

        [HttpGet("parent/{parentGroupId}/tree")]
        public async Task<IActionResult> GetGroupWithChildrenByParentTree(int parentGroupId, CancellationToken token)
        {
            return Ok(await Mediator.Send(new GroupsWithChildrenQuery{ParentGroupId = parentGroupId }, token));
        }

        [HttpGet("{groupId}/tree")]
        public async Task<IActionResult> GetGroupWithChildrenTree(int groupId, CancellationToken token)
        {
            return Ok(await Mediator.Send(new GroupWithChildrenQuery(groupId), token));
        }

        [HttpPost("{groupId}/add-member")]
        public async Task<IActionResult> AddGroupMember([FromBody] AddGroupMemberCommand command,
            CancellationToken token)
        {
            return Ok(await Mediator.Send(command, token));
        }
    }
}
