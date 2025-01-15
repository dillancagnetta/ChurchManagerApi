using ChurchManager.Api.Extensions;
using ChurchManager.Infrastructure.Persistence;
using ChurchManager.Infrastructure.Shared;
using ChurchManager.Infrastructure.Shared.SignalR.Hubs;
using CodeBoss.AspNetCore.DependencyInjection;
using Serilog;
using StartupBase = ChurchManager.Infrastructure.StartupBase;

namespace ChurchManager.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPersistenceInfrastructure(Configuration, Environment);
            services.AddSharedInfrastructure(Configuration, Environment);
            //services.AddApplicationLayer();

            services.InstallServicesInAssemblies(Configuration, Environment, typeof(Startup).Assembly);

            StartupBase.ConfigureServices(services, Configuration);

            // Add detection services container and device resolver service.
            services.AddDetection();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDetection();
            app.UseCors(ApiRoutes.DefaultCorsPolicy);
            app.UseSwaggerExtension();
            app.UseSerilogRequestLogging();

            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseErrorHandlingMiddleware();
            app.UseMultiTenant();

            app.UseHealthChecks(ApiRoutes.HealthChecks.DefaultUrl);

            app.UseEndpoints(endpoints =>
             {
                 endpoints.MapControllers();
                 endpoints.MapHub<NotificationHub>(ApiRoutes.Hubs.NotificationHub);
             });
        }
    }
}
