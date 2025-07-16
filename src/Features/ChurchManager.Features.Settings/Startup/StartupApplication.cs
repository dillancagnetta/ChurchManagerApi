using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.Settings;
using ChurchManager.Features.Settings.Services;
using ChurchManager.Infrastructure;
using ChurchManager.Infrastructure.Abstractions.AppContext;
using ChurchManager.Infrastructure.TypeSearcher;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Features.Settings.Startup
{
    public class StartupApplication : IStartupApplication
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISettingsService, SettingsService>();
            AddSettings(services);
        }

        public void Configure(IApplicationBuilder application, IWebHostEnvironment webHostEnvironment)
        {
        }

        public int Priority => 100;
        public bool BeforeConfigure => false;

        private void AddSettings(IServiceCollection services)
        {
            var typeSearcher = new TypeSearcher();
            var settings = typeSearcher.ClassesOfType<ISettings>();
            var instances = settings.Select(x => (ISettings)Activator.CreateInstance(x));
            foreach (var item in instances)
                services.AddScoped(item!.GetType(), x =>
                {
                    var type = item.GetType();
                    var tenantName = "";
                    var settingService = x.GetRequiredService<ISettingsService>();
                    var contextAccessor = x.GetRequiredService<IAppContextAccessor>();
                    if (contextAccessor.AppContext != null)
                    {
                        tenantName = contextAccessor.AppContext.CurrentTenant.Name;
                    }

                    return settingService.LoadSettingAsync(type, tenantName).Result;
                });
        }
    }
}
