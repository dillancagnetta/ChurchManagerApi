using ChurchManager.Infrastructure.Abstractions;
using ChurchManager.Infrastructure.Abstractions.AppContext;

namespace ChurchManager.Api.Middlewares;

public class ContextMiddleware
{
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

    private string? GetTenant(HttpContext context)
    {
        var tenantName = context.Request.Query["tenant"].ToString();

        if(string.IsNullOrWhiteSpace(tenantName))
        {
            if(context.User.Identity is not null && context.User.Identity.IsAuthenticated)
            {
                tenantName = context.User.Claims.FirstOrDefault(c => c.Type == "Tenant")?.Value;
            }
        }
        
        if(tenantName is not null)
        {
            _logger.LogInformation($"Tenant found in query string: {tenantName}");
        }
        
        return tenantName;
    }
}