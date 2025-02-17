using ChurchManager.Domain.Parameters;
using ChurchManager.Features.UserLogins.Commands.AddUserLogin;
using ChurchManager.Features.UserLogins.Queries;
using ChurchManager.SharedKernel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "System Admin")]
    public class UserLoginsController : BaseApiController
    {
        private readonly ILogger<UserLoginsController> _logger;
        private readonly ICognitoCurrentUser _currentUser;

        public UserLoginsController(
            ILogger<UserLoginsController> logger,
            ICognitoCurrentUser currentUser)
        {
            _logger = logger;
            _currentUser = currentUser;
        }

        #region Other

        // v1/userlogins/
        [HttpPost]
        public async Task<IActionResult> CreateUserLoginForPerson(AddOrUpdateUserLoginCommand command, CancellationToken token)
        {
            var response = await Mediator.Send(command, token);
            return Ok(response);
        }
        
        [HttpPost("browse")]
        public async Task<IActionResult> BrowseUserLogins(SearchTermQueryParameter filter, CancellationToken token)
        {
            var response = await Mediator.Send(new UserLoginsQuery(filter.SearchTerm), token);
            return Ok(response);
        }
        
        #endregion
    }
}
