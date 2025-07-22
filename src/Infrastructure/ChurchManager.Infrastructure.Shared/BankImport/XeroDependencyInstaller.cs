using ChurchManager.Domain.Features.Finances.Services;
using ChurchManager.Infrastructure.Abstractions.Finances;
using CodeBoss.AspNetCore.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Infrastructure.Shared.BankImport;

public class XeroDependencyInstaller: IDependencyInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        //services.Configure<XeroOptions>(configuration.GetSection(nameof(XeroOptions)));
        
        services.AddScoped<IBankStatementImporter, OfxBankStatementImporter>();
    }
}