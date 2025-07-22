//using ChurchManager.Application.ViewModels;

using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Security;

namespace ChurchManager.SharedKernel.Common
{
    // Marker for easy reference
    public interface IAppCurrentUser : ICognitoCurrentUser<PersonViewModel>
    {
        public string? Username { get; }
        public int PersonId { get; }
        public int? FamilyId { get; }
    }
}