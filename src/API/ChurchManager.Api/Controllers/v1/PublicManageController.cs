using ChurchManager.Features.Auth.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1;

[ApiVersion("1.0")]
public class PublicManageController : BaseApiController
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] PublicLoginCommand command, CancellationToken token)
    {
        var result = await Mediator.Send(command, token);
        
        return Ok(result);
    }
}