using CodeBoss.AspNetCore;
using CodeBoss.AspNetCore.DependencyInjection;

namespace ChurchManager.Api._DependencyInjection
{
    public class BuildVersionDependencyInstaller : IDependencyInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddSingleton<IBuildVersionInfo>(sp =>
            {
                var host = sp.GetRequiredService<IWebHostEnvironment>();
                return new BuildVersionInfo(host.ContentRootPath);
            });
        }
    }
}
