using ChurchManager.Domain.Shared;

namespace ChurchManager.Application.Abstractions.Services;

public interface ISecurityService
{
    Task<IEnumerable<UserLoginViewModel>> UserLoginsAsync(string searchTerm, CancellationToken ct = default);
    Task<IEnumerable<UserLoginRoleViewModel>> UserLoginRolesAsync(string searchTerm, CancellationToken ct = default);
}