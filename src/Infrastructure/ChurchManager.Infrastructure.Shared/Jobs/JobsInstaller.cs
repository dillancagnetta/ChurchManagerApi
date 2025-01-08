using CodeBoss.AspNetCore.DependencyInjection;
using CodeBoss.Jobs;
using CodeBoss.Jobs.Abstractions;
using Convey;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

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
            services.AddSingleton<ICodeBossJobListener, CmJobListener>();
            services.AddCodeBossJobs(configuration, typeof(CmServiceJobRepository), !environment.IsDevelopment(), registeredJobListener:true);
        }
    }
}