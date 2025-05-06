using ChurchManager.Domain.Features.Communications.Services;
using ChurchManager.Infrastructure.Abstractions.Communication;
using ChurchManager.Infrastructure.Abstractions.Configuration;
using ChurchManager.Infrastructure.Abstractions.Network;
using ChurchManager.Infrastructure.Shared.Templating;
using ChurchManager.Infrastructure.Shared.Templating.DataResolvers;
using CodeBoss.AspNetCore.DependencyInjection;
using Convey;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Infrastructure.Shared.Email
{
    public class CommunicationsDependencyInstaller : IDependencyInstaller
    {
        private const string AWS_ACCESS_KEY_ID = "AWS_ACCESS_KEY_ID";
        private const string AWS_SECRET_ACCESS_KEY = "AWS_SECRET_ACCESS_KEY";

        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            var emailSendingEnabled = configuration.GetOptions<AppConfig>("Application").EmailSendingEnabled;
            
            // Communications
            services.AddSingleton<ITemplateParser, DotLiquidTemplateParser>();

            if (emailSendingEnabled)
            {
                services.AddSingleton<IEmailSender>(sp =>
                {
                    // AWS Configuration
                    var accessKey = Environment.GetEnvironmentVariable(AWS_ACCESS_KEY_ID) ?? throw new ArgumentNullException(AWS_ACCESS_KEY_ID);
                    var secretKey = Environment.GetEnvironmentVariable(AWS_SECRET_ACCESS_KEY) ?? throw new ArgumentNullException(AWS_SECRET_ACCESS_KEY);
                    // var accessKey = configuration["AWS:AccessKey"];
                    //var secretKey = configuration["AWS:SecretKey"];

                    return new AwsSesEmailSender(accessKey, secretKey);
                } );
            }
            else
            {
                services.AddScoped<IEmailSender, FakeEmailSender>();
            }
            
            services.AddSingleton<IAwsIpRangeLoader, AwsIpRangeLoader>();
            
            services.AddScoped<IEmailOrchestrator, EmailOrchestrator>();

            services.AddTemplateDataResolvers();
        }
    }
}
