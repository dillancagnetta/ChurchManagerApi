﻿using ChurchManager.Features.UserLogins.Queries;
using ChurchManager.SharedKernel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "System Admin")]
    public class SecurityController(ICognitoCurrentUser currentUser) : BaseApiController
    {
        [HttpGet("roles")]
        public async Task<IActionResult> GetUserLoginRoles(string searchTerm, CancellationToken token)
        {
            var response = await Mediator.Send(new UserLoginRolesQuery(searchTerm), token);
            return Ok(response);
        }
        
        [HttpPost("role")]
        public async Task<IActionResult> CreateUserLoginRole(string searchTerm, CancellationToken token)
        {
            var response = await Mediator.Send(new UserLoginRolesQuery(searchTerm), token);
            return Ok(response);
        }
        
        [HttpPost("permissions/browse")]
        public async Task<IActionResult> BrowsePermissions([FromBody] BrowsePermissionsQuery query, CancellationToken token)
        {
            var response = await Mediator.Send(query, token);
            return Ok(response);
        }
    }
}
