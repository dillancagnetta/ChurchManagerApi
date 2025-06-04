using ChurchManager.Features.Auth.Commands;
using ChurchManager.Features.Communication.Queries.Preferences;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1;

[ApiVersion("1.0")]
[Authorize(Roles = "Public Access")]
public class PublicManageController : BaseApiController
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] PublicLoginCommand command, CancellationToken token)
    {
        var result = await Mediator.Send(command, token);
        
        return Ok(result);
    }
    
    [HttpGet("communication-preferences")]
    public async Task<IActionResult> GetCommunicationPreferences([FromQuery] int personId, CancellationToken token)
    {
        var result = await Mediator.Send(new GetCommunicationPreferencesQuery{PersonId = personId}, token);
        
        return Ok(result);
    }
}