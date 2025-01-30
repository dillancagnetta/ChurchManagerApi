using ChurchManager.Domain.Features.Communication.Services;
using ChurchManager.Infrastructure.Abstractions.Communication;
using ChurchManager.Infrastructure.Shared.Templating;
using CodeBoss.AspNetCore.DependencyInjection;
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
            // Communications
            services.AddSingleton<ITemplateParser, DotLiquidTemplateParser>();
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
    }
}
