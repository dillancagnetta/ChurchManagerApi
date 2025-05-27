using ChurchManager.Domain.Common;
using CodeBoss.MultiTenant;

namespace ChurchManager.Api.Middlewares;

// Registered as singletons in dotnet
public class TenantIdentifierMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TenantIdentifierMiddleware> _logger;

    public TenantIdentifierMiddleware(
        RequestDelegate next,
        ILogger<TenantIdentifierMiddleware> logger
    )
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var tenantName = context.Request.Query["tenant"].ToString();

        if(string.IsNullOrWhiteSpace(tenantName))
        {
            if(context.User.Identity is not null && context.User.Identity.IsAuthenticated)
            {
                tenantName = context.User.Claims.FirstOrDefault(c => c.Type == "Tenant")?.Value;
            }
        }
        // Get the tenant provider from the request scope because of scoping issues
        var tenantProvider = context.RequestServices.GetRequiredService<ITenantsProvider<TenantConfiguration>>();
        var tenant = tenantProvider.Get(tenantName);
        tenantProvider.CurrentTenant = tenant; // Set the current tenant in the request scope for other services to use

        if(tenant is not null)
        {
            _logger.LogInformation($"Tenant found in query string: {tenant.Name}");
        }

        await _next(context);
    }
}