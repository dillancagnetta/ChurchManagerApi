using ChurchManager.Domain.Features.Communications.Services;
using ChurchManager.Infrastructure.Abstractions;
using ChurchManager.Infrastructure.Abstractions.AppContext;
using ChurchManager.Infrastructure.Abstractions.Security;
using ChurchManager.Infrastructure.Shared.AppContext;
using ChurchManager.Infrastructure.Shared.Communications;
using ChurchManager.Infrastructure.Shared.DomainEvents;
using CodeBoss.AspNetCore.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ChurchManager.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            services.InstallServicesInAssemblies(configuration, environment, typeof(ServiceRegistration).Assembly);

            services.AddSingleton<ITokenService, TokenService>();

            services.AddScoped<IDomainEventPublisher, WolverineRabbitMqDomainEventPublisher>();
            
            services.AddDistributedMemoryCache();

            services.AddScoped<IMessageSender, MessageSender>();

            services.AddHttpClient();

            // Infrastructure Health Checks
            services.AddHealthChecks().AddCheck<RabbitMqHealthCheck>( 
                "rabbitmq-custom",
                failureStatus: HealthStatus.Degraded,
                tags: new[] { "infrastructure", "rabbitmq" });
            
            // Application Context
            services.AddSingleton<IAppContextAccessor, AppContextAccessor>();
            services.AddScoped<IAppContextSetter, AppContextSetter>();
        }
    }
}
