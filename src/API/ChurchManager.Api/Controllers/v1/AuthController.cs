using ChurchManager.Features.Auth.Commands;
using ChurchManager.SharedKernel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AuthController(ICognitoCurrentUser currentUser) : BaseApiController
    {
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken token)
        {
            var result = await Mediator.Send(command, token);

            if (!result.IsAuthenticated)
            {
                return Unauthorized();
            }

            return Ok(result);
        }
        
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(CancellationToken token)
        {
            if (currentUser.IsAuthenticated)
            {
                await Mediator.Send(new LogoutCommand(currentUser.Username), token);
            }
            
            return Ok();
        }
    }
}
