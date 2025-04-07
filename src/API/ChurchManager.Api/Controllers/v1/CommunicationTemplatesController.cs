using ChurchManager.Features.Communication.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class CommunicationTemplatesController(ILogger<CommunicationTemplatesController> logger) : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> GetSelectList([FromBody] CommunicationTemplatesSelectQuery query, CancellationToken token = default)
    {
        var templates = await Mediator.Send(query, token);
        
        return Ok(templates);
    }
}