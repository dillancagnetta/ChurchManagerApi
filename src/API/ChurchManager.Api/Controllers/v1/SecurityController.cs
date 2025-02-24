using ChurchManager.Features.UserLogins.Queries;
using ChurchManager.SharedKernel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "System Admin")]
    public class SecurityController(ICognitoCurrentUser currentUser) : BaseApiController
    {
        [HttpPost("roles")]
        public async Task<IActionResult> GetUserLoginRoles(UserLoginRolesQuery query, CancellationToken token)
        {
            var response = await Mediator.Send(query, token);
            return Ok(response);
        }
        
        [HttpPost("permissions")]
        public async Task<IActionResult> GetEntityPermissions(EntityPermissionsQuery query, CancellationToken token)
        {
            var response = await Mediator.Send(query, token);
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
        
        [HttpPost("permissions/create")]
        public async Task<IActionResult> CreatePermissions(CreatePermissionCommand command, CancellationToken token)
        {
            var response = await Mediator.Send(command, token);
            return Ok(response);
        }
        
        [HttpPost("permissions/add-to-role")]
        public async Task<IActionResult> AddPermissionsToRole(AddPermissionsToRoleCommand command, CancellationToken token)
        {
            var response = await Mediator.Send(command, token);
            return Ok(response);
        }
    }
}
