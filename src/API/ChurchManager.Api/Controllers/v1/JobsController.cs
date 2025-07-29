using ChurchManager.Features.Jobs.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class JobsController : BaseApiController
{
    [HttpPost]  
    public async Task<IActionResult> Browse(BrowseJobsQuery query, CancellationToken token = default)
    {
        var response = await Mediator.Send(query, token);
            
        return Ok(response);  
    }
}