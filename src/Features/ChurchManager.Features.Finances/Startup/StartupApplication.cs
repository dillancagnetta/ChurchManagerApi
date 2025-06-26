using ChurchManager.Domain.Features.Finances.Services;
using ChurchManager.Features.Finances.Services;
using ChurchManager.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Features.Finances.Startup;

public class StartupApplication: IStartupApplication
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IGivingReferenceResolver, GivingReferenceResolver>();
    }

    public void Configure(IApplicationBuilder application, IWebHostEnvironment webHostEnvironment)
    {
        throw new NotImplementedException();
    }

    public int Priority => 100;
    public bool BeforeConfigure => false;
}