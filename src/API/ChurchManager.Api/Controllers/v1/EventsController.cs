using ChurchManager.Domain.Features.Events.Repositories;
using ChurchManager.SharedKernel.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class EventsController(IEventDbRepository dbRepository) : BaseApiController
    {

        [HttpGet("{eventId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEventDetails(int eventId, CancellationToken token)
        {
            return Ok(new ApiResponse(await dbRepository.EventDetailsAsync(eventId, token)));
        }
    }
}
