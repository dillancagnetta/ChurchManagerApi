using ChurchManager.Features.Churches.Queries.RetrieveChurchGroups;
using ChurchManager.SharedKernel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class ChurchGroupsController : BaseApiController
    {
        private readonly ICognitoCurrentUser _currentUser;

        public ChurchGroupsController(ICognitoCurrentUser currentUser)
        {
            _currentUser = currentUser;
        }
       
        [HttpPost("browse")]
        public async Task<IActionResult> Browse([FromBody] BrowseChurchGroups query, CancellationToken token)
        {
            return Ok(await Mediator.Send(query, token));
        }
        
        /*[HttpGet]
        public async Task<IActionResult> GetAll( CancellationToken token)
        {
            return Ok(await Mediator.Send(new GetGroupTypeQuery(groupTypeId), token));
        }
            
        [HttpGet("{groupTypeId}")]
        public async Task<IActionResult> GetGroupTypeById(int groupTypeId, CancellationToken token)
        {
            return Ok(await Mediator.Send(new GetGroupTypeQuery(groupTypeId), token));
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddGroupTypeCommand cmd, CancellationToken token)
        {
            return Accepted(await Mediator.Send(cmd, token));
        }
        
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EditGroupTypeCommand cmd, CancellationToken token)
        {
            return Accepted(await Mediator.Send(cmd, token));
        }
        
        [HttpDelete("{groupTypeId}")]
        public async Task<IActionResult> Delete(int groupTypeId, CancellationToken token)
        {
            return Ok(await Mediator.Send(new DeleteGroupTypeCommand(groupTypeId), token));
        }*/
    }
}
