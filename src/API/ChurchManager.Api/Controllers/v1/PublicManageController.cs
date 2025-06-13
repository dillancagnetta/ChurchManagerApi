using ChurchManager.Features.Auth.Commands;
using ChurchManager.Features.Communication.Queries.Preferences;
using ChurchManager.Features.People.Commands.ChangeRequests;
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

    [HttpPut("baptism-update")]
    public async Task<IActionResult> UpdateBaptismStatus(RequestBaptismChangeCommand command, CancellationToken ct)
    {
        await Mediator.Send(command, ct);
        
        return Accepted();
    }
    
    [HttpPut("holy-spirit-update")]
    public async Task<IActionResult> UpdateReceivedHolySpirit(ReceivedHolySpiritChangeCommand command, CancellationToken ct)
    {
        await Mediator.Send(command, ct);
        
        return Accepted();
    }
    
    [HttpPut("personal-info-update")]
    public async Task<IActionResult> UpdatePersonalInfo(RequestBaptismChangeCommand command, CancellationToken ct)
    {
        await Mediator.Send(command, ct);
        
        return Accepted();
    }
}