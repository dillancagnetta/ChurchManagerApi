using System.Security.Claims;

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
                new Claim(ClaimTypes.NameIdentifier, "test-user-123"),
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim("Tenant", "test-tenant"),
                new Claim("FamilyId", "999")
            };

            var identity = new ClaimsIdentity(claims, "Testing");
            context.User = new ClaimsPrincipal(identity);
        }
#endif

        await _next(context);
    }
}