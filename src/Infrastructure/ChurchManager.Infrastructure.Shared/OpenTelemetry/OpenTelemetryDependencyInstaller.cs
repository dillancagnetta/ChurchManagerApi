﻿using CodeBoss.AspNetCore.DependencyInjection;
using Convey;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ChurchManager.Infrastructure.Shared.OpenTelemetry
{
    public class OpenTelemetryDependencyInstaller : IDependencyInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            // Get from configuration
            var telemetryOptions = configuration.GetOptions<OpenTelemetryOptions>(nameof(OpenTelemetryOptions));
            // Add to DI
            services.Configure<OpenTelemetryOptions>(configuration.GetSection(nameof(OpenTelemetryOptions)));

            if (telemetryOptions.Enabled)
            {
                // Configure
                /*services.Configure<AspNetCoreInstrumentationOptions>(options =>
                {
                    options.Filter = (httpContext) =>
                    {
                        // only collect telemetry about the following requests
                        var path = httpContext.Request.Path;
                        return path.HasValue && (
                            !path.Value.Contains("aspnetcore-browser-refresh") &&
                            !path.Value.Contains("swagger")
                        );
                    };
                });*/

                /*services.AddOpenTelemetry(builder => builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("ChurchManager"))
                    .AddSource("ChurchManager.Application") // when we manually create activities, we need to setup the sources here
                    .AddAspNetCoreInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation(options => options.SetDbStatementForText = true)
                    .AddJaegerExporter(options =>
                    {
                        // default is "localhost"
                        options.AgentHost = telemetryOptions.Host ?? "localhost";
                        //options.AgentPort = 6831;
                    })
                );*/

                services.AddOpenTelemetry()
                    .ConfigureResource(r => r
                        .AddService(
                            serviceName: "ChurchManager",
                            serviceVersion: "1.0.0",
                            serviceInstanceId: telemetryOptions.Host))
                    .WithTracing(builder =>
                        {
                            builder.AddSource("ChurchManager.Application")
                                .AddEntityFrameworkCoreInstrumentation(options => options.SetDbStatementForText = true)
                                //.AddHttpClientInstrumentation()
                                .AddAspNetCoreInstrumentation(options =>
                                {
                                    options.Filter = (httpContext) =>
                                    {
                                        // only collect telemetry about the following requests
                                        var path = httpContext.Request.Path;
                                        return path.HasValue && (
                                            !path.Value.Contains("aspnetcore-browser-refresh") &&
                                            !path.Value.Contains("swagger")
                                        );
                                    };
                                });
                            ; // when we manually create activities, we need to setup the sources here
                            builder.AddJaegerExporter(options =>
                            {
                                // default is "localhost"
                                options.AgentHost = telemetryOptions.Host ?? "localhost";
                                //options.AgentPort = 6831;
                            });
                        }
                    );
            }
        }
    }
}
