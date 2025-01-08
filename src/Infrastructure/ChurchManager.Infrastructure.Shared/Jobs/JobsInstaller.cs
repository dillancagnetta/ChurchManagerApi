using CodeBoss.AspNetCore.DependencyInjection;
using CodeBoss.Jobs;
using Convey;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Infrastructure.Shared.Jobs;

public class JobsInstaller : IDependencyInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        // Get from configuration
        var options = configuration.GetOptions<JobsOptions>(nameof(JobsOptions));
        // Add to DI
        services.Configure<JobsOptions>(configuration.GetSection(nameof(JobsOptions)));
        
        if (options.Enabled)
        {
            services.AddCodeBossJobs(configuration, typeof(CmServiceJobRepository), !environment.IsDevelopment());
        }
    }
}