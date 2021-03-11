﻿using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Features.Profile.Queries.RetrieveProfile;
using ChurchManager.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        // v1/profiles/userdetails/{{userLoginId}}
        [HttpGet("userdetails/{userLoginId}")]
        public async Task<IActionResult> GetUserDetailsSummaryByUserLogin(string userLoginId, CancellationToken token)
        {
            var response = await Mediator.Send(new UserDetailsByUserLoginQuery(userLoginId), token);
            return Ok(response);
        } 

        #endregion

        #region Current User

        // v1/profiles/current-user
        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUserProfileByUserLogin(CancellationToken token)
        {
            var response = await GetUserProfileByUserLogin(_currentUser.UserLoginId, token);
            return Ok(response);
        }

        // v1/profiles/userdetails/current-user
        [HttpGet("userdetails/current-user")]
        public async Task<IActionResult> GetCurrentUserDetailsSummaryByUserLogin(string userLoginId, CancellationToken token)
        {
            var response = await GetUserDetailsSummaryByUserLogin(_currentUser.UserLoginId, token);
            return Ok(response);
        } 

        #endregion
    }
}
