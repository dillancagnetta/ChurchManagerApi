using ChurchManager.Infrastructure.Abstractions.AppContext;
using CodeBoss.Extensions;

namespace ChurchManager.Api.Middlewares;

public class ContextMiddleware
{
    private readonly List<string> _skipRoutePattern = ["/openapi/{documentName}.json"];

    private readonly RequestDelegate _next;
    private readonly IAppContextAccessor _contextAccessor;
    private readonly ILogger<ContextMiddleware> _logger;

    public ContextMiddleware(
        RequestDelegate next,
        IAppContextAccessor contextAccessor,
        ILogger<ContextMiddleware> logger
    )
    {
        _next = next;
        _contextAccessor = contextAccessor;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        if (context?.Request == null) return;
        
        var endpoint = context.GetEndpoint();
        if (endpoint != null)
        {
            var routePattern = (endpoint as RouteEndpoint)?.RoutePattern.RawText;
            if (routePattern != null && _skipRoutePattern.Any(pattern => routePattern.StartsWith(pattern, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }
        }
        
        var subdomain = GetSubdomain(context);
        var tenantName = GetTenant(context);
        
        var appContext = context.RequestServices.GetRequiredService<IAppContextSetter>();
        _contextAccessor.AppContext = await appContext.InitializeAppContext(subdomain, tenantName);
      
        await _next(context);
    }
    
    private string GetSubdomain(HttpContext context)
    {
        var host = context.Request.Host.Host;
        
        if (host == "localhost") return host;
     
        var parts = host.Split('.');
        if (parts.Length > 2)
        {
            return parts[0];
        }
        return host;
    }

    /// <summary>
    /// Extracts the tenant name from the HTTP request using multiple fallback strategies.
    /// First checks the query string parameter "tenant", then authenticated user claims,
    /// and finally the "X-Tenant" HTTP header.
    /// </summary>
    private string? GetTenant(HttpContext context)
    {
        // Check query string for tenant name
        var tenantName = context.Request.Query["tenant"].ToString();

        if(string.IsNullOrWhiteSpace(tenantName))
        {
            if(context.User.Identity is not null && context.User.Identity.IsAuthenticated)
            {
                // Check JWT token for tenant name
                tenantName = context.User.Claims.FirstOrDefault(c => c.Type == "Tenant")?.Value;
            }
        }

        if (string.IsNullOrWhiteSpace(tenantName))
        {
            // Check X-Tenant header
            tenantName = context.Request.Headers["X-Tenant"].ToString();
        }

        if(!tenantName.IsNullOrEmpty())
        {
            _logger.LogInformation($"Tenant found: [{tenantName}]");
        }
        
        return tenantName;
    }
}