using CodeBoss.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ChurchManager.Api.Middlewares
{
    // The actual filter implementation with DI support
    public class AllowedDomainsFilter : IActionFilter
    {
        private readonly IList<string> _allowedDomains = new List<string>(1);

        public AllowedDomainsFilter(IConfiguration configuration, string[] allowedDomains)
        {
            // Get domains from appsettings first
            var configDomains = configuration.GetSection("AllowedDomains").Get<string[]>();
            
            if (!configDomains.IsNullOrEmpty())
            {
                _allowedDomains = configDomains!.ToList();
            }
            else if (!allowedDomains.IsNullOrEmpty())
            {
                _allowedDomains = allowedDomains.ToList();
            }
            else
            {
                _allowedDomains = new List<string>{ "codeboss.co.za" }; // Default allowed domains
            }
            
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "";
            if (environment.Equals("Development", StringComparison.OrdinalIgnoreCase))
            {
                _allowedDomains.Add("localhost"); // Add development domain for Angular PWA test    
                _allowedDomains.Add("127.0.0.1");
                _allowedDomains.Add("::1"); // IPv6 localhost
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var origin = request.Headers["Origin"].ToString();
            var referer = request.Headers["Referer"].ToString();

            var invalidOrigin = string.IsNullOrEmpty(origin) ||
                                !_allowedDomains.Any(domain =>
                                    origin.Contains(domain, StringComparison.OrdinalIgnoreCase));
            var invalidReferer = string.IsNullOrEmpty(origin) ||
                                !_allowedDomains.Any(domain =>
                                    referer.Contains(domain, StringComparison.OrdinalIgnoreCase));
            
            if (invalidOrigin || invalidReferer)
            {
                context.Result = new ForbidResult();
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No implementation needed for this filter
        }
    }

    // The attribute that uses TypeFilter to create the filter
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AllowedDomainsAttribute : TypeFilterAttribute
    {
        public AllowedDomainsAttribute(params string[] allowedDomains) 
            : base(typeof(AllowedDomainsFilter))
        {
            Arguments = new object[] { allowedDomains };
        }
    }
}