using CodeBoss.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ChurchManager.Api.Middlewares
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AllowedDomainsAttribute : ActionFilterAttribute
    {
        private IList<string> _allowedDomains = new List<string>(1);

        public AllowedDomainsAttribute(params string[] allowedDomains)
        {
            if (allowedDomains.IsNullOrEmpty())
            {
                _allowedDomains = new List<string>{ "codeboss.co.za" }; // Default allowed domains
            }
            else
            {
                _allowedDomains = allowedDomains;
            }
            
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "";
            if (environment.Equals("Development", StringComparison.OrdinalIgnoreCase))
            {
                _allowedDomains.Add("localhost"); // Add development domain for Angular PWA test    
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var origin = request.Headers["Origin"].ToString();

            if (string.IsNullOrEmpty(origin) || !_allowedDomains.Any(domain => origin.Contains(domain, StringComparison.OrdinalIgnoreCase)))
            {
                context.Result = new ForbidResult();
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}