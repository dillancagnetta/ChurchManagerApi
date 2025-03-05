using ChurchManager.Features.Churches.Queries.RetrieveChurches;
using ChurchManager.SharedKernel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class ChurchesController : BaseApiController
{
    private readonly ICognitoCurrentUser _currentUser;

    public ChurchesController(ICognitoCurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<IActionResult> AllChurches(CancellationToken token)
    {
        var groups = await Mediator.Send(new ChurchesQuery(), token);
        return Ok(groups);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddChurchCommand cmd, CancellationToken token)
    {
        return Ok(await Mediator.Send(cmd, token));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken token)
    {
        return Ok(await Mediator.Send(new DeleteChurchCommand(id), token));
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] EditChurchCommand cmd, CancellationToken token)
    {
        return Accepted(await Mediator.Send(cmd, token));
    }
}