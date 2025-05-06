using System.Net;
using System.Security.Cryptography.X509Certificates;
using Amazon.Runtime;
using Convey;
using Convey.Logging;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace ChurchManager.Api
{
    public class Program
    {
        public const string AppName = "ChurchManager";

        public static void Main(string[] args)
        {
            Console.Title = AppName;
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(services => services.AddConvey().Build())
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddEnvironmentVariables();
                } )
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                 
                    webBuilder.ConfigureKestrel((context, options) => ConfigureKestrelHttp(context, options));
                })
                .ConfigureAppConfiguration((context, config) =>
                {
                    var environmentName = context.HostingEnvironment.EnvironmentName;
                   
                    // Validate Environment Variables Needed
                    ValidateEnvironmentVariables(environmentName);

                    // Pull settings from AWS parameter store
                    ConfigureAwsParameterStore(config, environmentName);

                    // https://github.com/npgsql/efcore.pg/issues/2000
                    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                })
                .UseLogging();

            void ConfigureAwsParameterStore(IConfigurationBuilder configurationBuilder, string environment)
            {
                configurationBuilder
                    .AddEnvironmentVariables()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", true, true);

                var configuration = configurationBuilder.Build();

                // AWS Configuration
                var awsOptions = configuration.GetAWSOptions();
                //var accessKey = configuration["AWS:AccessKey"];
                //var secretKey = configuration["AWS:SecretKey"];
                //awsOptions.Credentials = new BasicAWSCredentials(accessKey, secretKey);
                awsOptions.Credentials = new EnvironmentVariablesAWSCredentials();

                // AWS Parameter Store
                configurationBuilder.AddSystemsManager(
                    path: $"/{AppName}/{environment}",
                    awsOptions: awsOptions,
                    reloadAfter: TimeSpan.FromMinutes(5));
            }

            void ValidateEnvironmentVariables(string environment)
            {
                Console.WriteLine($"** Environment: [{environment}], " +
                                  $"ASPNETCORE_ENVIRONMENT: [{Environment.GetEnvironmentVariable(ASPNETCORE_ENVIRONMENT)}]  " +
                                  $"AWS_ACCESS_KEY_ID : [{Environment.GetEnvironmentVariable(AWS_ACCESS_KEY_ID)}]  " +
                                  $"AWS_REGION  : [{Environment.GetEnvironmentVariable(AWS_REGION)}]  "
                );

                _ = Environment.GetEnvironmentVariable(ASPNETCORE_ENVIRONMENT) ?? throw new ArgumentNullException(ASPNETCORE_ENVIRONMENT);
                _ = Environment.GetEnvironmentVariable(AWS_ACCESS_KEY_ID) ?? throw new ArgumentNullException(AWS_ACCESS_KEY_ID);
                _ = Environment.GetEnvironmentVariable(AWS_SECRET_ACCESS_KEY) ?? throw new ArgumentNullException(AWS_SECRET_ACCESS_KEY);
                _ = Environment.GetEnvironmentVariable(AWS_REGION) ?? throw new ArgumentNullException(AWS_REGION);
            }
        }

        private static void ConfigureKestrelHttp(WebHostBuilderContext context, KestrelServerOptions options)
        {
            if (context.HostingEnvironment.EnvironmentName != "Development")
            {
                // Read environment variables for port configuration
                var httpPortString = Environment.GetEnvironmentVariable("ASPNETCORE_HTTP_PORTS") ?? "8080";
                var httpsPortString = Environment.GetEnvironmentVariable("ASPNETCORE_HTTPS_PORTS") ?? "443";
                            
                // Configure HTTP endpoints
                if (int.TryParse(httpPortString, out var httpPort))
                {
                    options.Listen(IPAddress.Any, httpPort);
                }

                // Configure HTTPS endpoints with certificate
                if (int.TryParse(httpsPortString, out var httpsPort))
                {
                    options.ListenAnyIP(httpsPort, listenOptions =>
                    {
                        listenOptions.UseHttps(httpsOptions =>
                        {
                            // Load the certificate from the container path
                            /*httpsOptions.ServerCertificate = new X509Certificate2(
                                "certificate.pfx", 
                                "YourStrongPassword",
                                X509KeyStorageFlags.MachineKeySet | 
                                X509KeyStorageFlags.PersistKeySet | 
                                X509KeyStorageFlags.Exportable
                            );*/
                            // Load the certificate from the container path
                            httpsOptions.ServerCertificate = X509CertificateLoader.LoadPkcs12FromFile("certificate.pfx", 
                                "YourStrongPassword",
                                X509KeyStorageFlags.MachineKeySet | 
                                X509KeyStorageFlags.PersistKeySet | 
                                X509KeyStorageFlags.Exportable);
                        });
                                    
                        // Enable HTTP/2/3
                        listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
                    }); 
                }
            }
        }

        private const string ASPNETCORE_ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";
        private const string AWS_ACCESS_KEY_ID = "AWS_ACCESS_KEY_ID";
        private const string AWS_SECRET_ACCESS_KEY = "AWS_SECRET_ACCESS_KEY";
        private const string AWS_REGION = "AWS_REGION";
    }
}