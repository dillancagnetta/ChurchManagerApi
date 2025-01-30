using ChurchManager.Api.Extensions;
using ChurchManager.Api.Middlewares;
using CodeBoss.AspNetCore.DependencyInjection;

namespace ChurchManager.Api._DependencyInjection
{
    public class AspNetDependencyInstaller : IDependencyInstaller
    {
        private const string Prefix = "api";

        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddCorsExtension();
            services.AddControllersExtension(Prefix);
            services.AddSwaggerExtension();

            // API version
            services.AddApiVersioningExtension();
            // API explorer
            services.AddMvcCore(options => {
                    options.Filters.Add<OperationCancelledExceptionFilter>();
                })
                .AddApiExplorer();
            // API explorer version
            services.AddVersionedApiExplorerExtension();

            services.AddHealthChecks();
            
            // Does not register it globally but only where it is applied
            services.AddScoped<AwsIpFilterAttribute>();
        }
    }
}
