using System.Security.Claims;
using ChurchManager.Infrastructure.Persistence.Seeding;

namespace ChurchManager.Api.Authorization.AllowTesting;

public class AllowTestingMiddleware
{
    private readonly RequestDelegate _next;

    public AllowTestingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Only in Development
#if DEBUG
        var endpoint = context.GetEndpoint();
        var hasAllowTesting = endpoint?.Metadata.GetMetadata<AllowTestingAttribute>() != null;

        if (hasAllowTesting && !context.User.Identity.IsAuthenticated)
        {
            // Create a simple test user
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, SeedingConstants.MainUserLogin),
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim("Tenant", SeedingConstants.DemoTenantName),
                new Claim("FamilyId", "1"),
                new Claim("PersonId", "1")
            };

            var identity = new ClaimsIdentity(claims, "Testing");
            context.User = new ClaimsPrincipal(identity);
        }
#endif

        await _next(context);
    }
}