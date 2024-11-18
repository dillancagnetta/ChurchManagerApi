using ChurchManager.Features.History.Queries.RetrieveHistory;
using ChurchManager.SharedKernel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class HistoryController : BaseApiController
{
    private readonly ILogger<HistoryController> _logger;
    private readonly ICognitoCurrentUser _currentUser;
    
    public HistoryController(
        ILogger<HistoryController> logger,
        ICognitoCurrentUser currentUser)
    {
        _logger = logger;
        _currentUser = currentUser;
    }
    
    [HttpGet]
    public async Task<IActionResult> BrowseHistory(
        [FromQuery]string entityType, 
        [FromQuery]int entityId, 
        [FromQuery]int page, 
        [FromQuery]int results, 
        CancellationToken token)
    {
        var query = new BrowseHistory(entityType, entityId)
        {
            Page = page,
            Results = results
        };
        var response = await Mediator.Send(query, token);
        return Ok(response);
    }
    
    // v1/history/current-user
    [HttpGet("current-user")]
    public async Task<IActionResult> BrowseCurrentUserHistoryByPersonId(
        [FromQuery]string entityType,
        [FromQuery]int page, 
        [FromQuery]int results, 
        CancellationToken token)
    {
        var query = new BrowseCurrentUserHistory(entityType, _currentUser.PersonId)
        {
            Page = page,
            Results = results
        };
        var response = await Mediator.Send(query, token);
        return Ok(response);
    }
}