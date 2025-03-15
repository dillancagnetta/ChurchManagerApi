using ChurchManager.Domain.Features.Events.Repositories;
using ChurchManager.Features.Events.Commands;
using ChurchManager.Features.Events.Queries.GetEvents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class EventsTypesController(IEventDbRepository dbRepository) : BaseApiController
{
        
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDetails = false, CancellationToken token = default)
    {
        return Ok(await Mediator.Send(new EventsQuery{IncludeDetails = includeDetails}, token));
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddEventTypeCommand cmd, CancellationToken token)
    {
        return Ok(await Mediator.Send(cmd, token));
    }
        
}