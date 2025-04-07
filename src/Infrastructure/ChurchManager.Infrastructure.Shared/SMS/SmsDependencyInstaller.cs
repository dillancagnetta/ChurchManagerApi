using System.Net.Http.Headers;
using System.Text;
using ChurchManager.Domain.Features.Communications.Services;
using ChurchManager.Infrastructure.Abstractions.Configuration;
using CodeBoss.AspNetCore.DependencyInjection;
using Convey;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Infrastructure.Shared.SMS;

public class SmsDependencyInstaller: IDependencyInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var smsEnabled = configuration.GetOptions<AppConfig>("Application").SMSSendingEnabled;
        if (smsEnabled)
        {
            // Get from configuration
            var options = configuration.GetOptions<BulkSmsOptions>(nameof(BulkSmsOptions));
            // Add to DI
            services.Configure<BulkSmsOptions>(configuration.GetSection(nameof(BulkSmsOptions)));

            string authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{options.TokenId}:{options.TokenSecret}"));
            
            // Register HttpClient with configuration
            services.AddHttpClient("BulkSmsClient", (serviceProvider, client) =>
            {
                client.BaseAddress = new Uri(options.ApiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
            });

            services.AddScoped<ISmsSender, BulkSmsSender>();
        }
    }
}