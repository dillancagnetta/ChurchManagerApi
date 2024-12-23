﻿using ChurchManager.Features.Profile.Commands.IncrementProfileViewed;
using ChurchManager.Features.Profile.Queries.RetrieveProfile;
using ChurchManager.SharedKernel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class ProfilesController : BaseApiController
    {
        private readonly ILogger<ProfilesController> _logger;
        private readonly ICognitoCurrentUser _currentUser;

        public ProfilesController(
            ILogger<ProfilesController> logger,
            ICognitoCurrentUser currentUser)
        {
            _logger = logger;
            _currentUser = currentUser;
        }

        #region Other

        // v1/profiles/userlogin/{{userLoginId}}
        [HttpGet("userlogin/{userLoginId}")]
        public async Task<IActionResult> GetUserProfileByUserLogin(string userLoginId, CancellationToken token)
        {
            var response = await Mediator.Send(new ProfileByUserLoginIdQuery(userLoginId), token);
            return Ok(response);
        }

        // v1/profiles/person/{{personId}}
        [HttpGet("person/{personId}")]
        public async Task<IActionResult> GetUserProfileByPerson(int personId, [FromQuery] bool condensed, CancellationToken token)
        { 
            var response = await Mediator.Send(new ProfileByPersonIdQuery(personId, condensed), token);
            // Increment counter
            Mediator.Send(new IncrementProfileViewedCommand(personId), token);
            
            return Ok(response);
        }

        #endregion

        #region Current User

        // v1/profiles/current-user
        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUserProfileByUserLogin(CancellationToken token)
        {
            var response = await Mediator.Send(new ProfileByUserLoginIdQuery(_currentUser.Id), token);
            return Ok(response);
        }

        #endregion
    }
}
