using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.Security.Services;
using ChurchManager.Features.Auth.Services;
using ChurchManager.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Features.Auth.Startup
{
    public class StartupApplication : IStartupApplication
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IPermissionContext, PermissionContext>();
            services.AddScoped<ISecurityService, SecurityService>();
        }

        public void Configure(IApplicationBuilder application, IWebHostEnvironment webHostEnvironment)
        {
        }

        public int Priority => 100;
        public bool BeforeConfigure => false;
    }
}
