using ChurchManager.Domain.Features.Events.Repositories;
using ChurchManager.Features.Events.Commands;
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
        
        [HttpPost("{eventId}/register-for-event")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterForEvent([FromBody] RegisterFamilyForEventCommand command, CancellationToken token)
        {
            return Ok(await Mediator.Send(command, token));
        }
    }
}
