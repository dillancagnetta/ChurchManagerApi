﻿/*
using ChurchManager.Infrastructure.Shared.SignalR.Hubs;
using ChurchManager.Infrastructure.Shared.Tests;
using CodeBoss.AspNetCore.DependencyInjection;
using MassTransit;
using MassTransit.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChurchManager.Infrastructure.Shared._DependencyInjection
{
    public class MassTransitDependencyInstaller : IDependencyInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            var connectionString = configuration.GetConnectionString("RabbitMq");

            #region MassTransit

            services.AddMassTransit(x =>
            {
                x.AddConsumers(typeof(TestDomainEventConsumer).Assembly);

                // ** Add Hubs Here **
                x.AddSignalRHub<NotificationHub>();

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(connectionString), h => { });

                    cfg.ConfigureEndpoints(provider, new SnakeCaseEndpointNameFormatter(false));
                }));

            });

            // services.AddMassTransitHostedService();

            #endregion
        }
    }
}
*/
