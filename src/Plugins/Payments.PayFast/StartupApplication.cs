using ChurchManager.Domain.Features.Finances.Services;
using ChurchManager.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payments.PayFast.Services;

namespace Payments.PayFast;

public class StartupApplication: IStartupApplication
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPaymentProvider, PayFastPaymentProvider>();
        services.AddScoped<IPayFastService, PayFastService>();
    }

    public void Configure(IApplicationBuilder application, IWebHostEnvironment webHostEnvironment)
    {
    }

    public int Priority => 10;    
    public bool BeforeConfigure => false;
}