﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ChurchManager.Application.Features.People.Services;
using ChurchManager.Domain;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Model;
using Codeboss.Types;

namespace ChurchManager.Application
{
    public class CognitoCurrentUser : ICognitoCurrentUser
    {
        private readonly ICurrentPrincipalAccessor _principalAccessor;
        private readonly IPersonService _applicationService;

        internal const string ClaimTypeUsername = "Username";

        public CognitoCurrentUser(ICurrentPrincipalAccessor principalAccessor, IPersonService applicationService)
        {
            _principalAccessor = principalAccessor;
            _applicationService = applicationService;
        }

        public virtual bool IsAuthenticated => !string.IsNullOrEmpty(Id);
        public string Id => _principalAccessor.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
        public string Username => _principalAccessor.Principal.FindFirstValue(ClaimTypeUsername);
        public int PersonId => CurrentPerson.Value.GetAwaiter().GetResult().PersonId;

        /// <summary>
        /// Loads the Current User using the
        /// </summary>
        public Lazy<Task<PersonDomain>> CurrentPerson => new(async () => await _applicationService.PersonByUserLoginId(Id));
    }
}
