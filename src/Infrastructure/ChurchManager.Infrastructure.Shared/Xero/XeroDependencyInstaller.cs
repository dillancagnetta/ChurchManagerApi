using ChurchManager.Infrastructure.Abstractions.Finances;
using CodeBoss.AspNetCore.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WebPush;

namespace ChurchManager.Infrastructure.Shared.Xero;

public class XeroDependencyInstaller: IDependencyInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.Configure<XeroOptions>(configuration.GetSection(nameof(XeroOptions)));
        
        services.AddScoped<IExternalFinanceIntegrator, XeroFinanceService>();
    }
}