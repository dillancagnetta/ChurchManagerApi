using ChurchManager.Features.Common.Commands;
using ChurchManager.Features.Common.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class ChangeRequestsController : BaseApiController
{
    [HttpPost("approve")]
    public async Task<IActionResult> ApproveChangeRequest([FromBody] ApproveChangeRequestCommand command, CancellationToken token)
    {
        return Ok(await Mediator.Send(command, token));
    }
    
    [HttpPost("browse")]
    public async Task<IActionResult> Browse([FromBody] BrowseChangeRequestsQuery query, CancellationToken token)
    {
        return Ok(await Mediator.Send(query, token));
    }
    
    [HttpPost]
    public async Task<IActionResult> AllChangeRequests(GetChangeRequestsQuery query, CancellationToken token)
    {
        var vm = await Mediator.Send(query, token);
        return Ok(vm);
    }
}