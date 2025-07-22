using System.Security.Claims;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Shared;
//using ChurchManager.Application.ViewModels;
using ChurchManager.SharedKernel.Common;
using CodeBoss.Extensions;
using Codeboss.Types;

namespace ChurchManager.Features.Common
{
    public class AppCurrentUser : IAppCurrentUser
    {
        private const string ClaimTypeUsername = ClaimTypes.Name;
        private readonly IProfileService _applicationService;
        private readonly ICurrentPrincipalAccessor _principalAccessor;

        public AppCurrentUser(ICurrentPrincipalAccessor principalAccessor, IProfileService applicationService)
        {
            _principalAccessor = principalAccessor;
            _applicationService = applicationService;
        }

        public virtual bool IsAuthenticated => _principalAccessor.Principal?.Identity?.IsAuthenticated ?? false;
        public string? Id => _principalAccessor.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
        public string? Username => _principalAccessor.Principal.FindFirstValue(ClaimTypeUsername);
        public int PersonId => CurrentPerson.Value.GetAwaiter().GetResult().PersonId;
        public int? FamilyId => _principalAccessor.Principal.FindFirstValue("FamilyId")?.AsIntegerOrNull();

        /// <summary>
        ///     Loads the Current User using the
        /// </summary>
        public Lazy<Task<PersonViewModel>> CurrentPerson =>
            new(async () => await _applicationService.ProfileByUserLoginId(Id!));
    }
}