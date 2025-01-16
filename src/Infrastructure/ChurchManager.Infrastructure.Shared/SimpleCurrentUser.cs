using System.Security.Claims;
using CodeBoss.MultiTenant;
using Codeboss.Types;

namespace ChurchManager.Infrastructure.Shared
{
    public class SimpleCurrentUser : ITenantCurrentUser
    {
        private readonly ICurrentPrincipalAccessor _principalAccessor;

        public SimpleCurrentUser(ICurrentPrincipalAccessor principalAccessor) => _principalAccessor = principalAccessor;

        public virtual bool IsAuthenticated => !string.IsNullOrEmpty(Id);
        public string Id => _principalAccessor?.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
        public string Tenant => _principalAccessor?.Principal?.FindFirstValue("Tenant");
    }
}
