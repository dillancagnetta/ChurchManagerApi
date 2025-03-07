using ChurchManager.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Features.Events.Startup;

public class StartupApplication: IStartupApplication
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
    }

    public void Configure(IApplicationBuilder application, IWebHostEnvironment webHostEnvironment)
    {
    }

    public int Priority => 101;
    public bool BeforeConfigure => false;
}