﻿using System.Reflection;
using AutoMapper;
using ChurchManager.Infrastructure.Abstractions.Configuration;
using ChurchManager.Infrastructure.Mapper;
using ChurchManager.Infrastructure.Plugins;
using ChurchManager.Infrastructure.Roslyn;
using ChurchManager.Infrastructure.Shared.SignalR.Hubs;
using ChurchManager.Infrastructure.Shared.Tests;
using ChurchManager.Infrastructure.TypeConverters;
using ChurchManager.Infrastructure.TypeSearcher;
using ChurchManager.SharedKernel;
using ChurchManager.SharedKernel.Extensions;
using FluentValidation.AspNetCore;
using MassTransit;
using MassTransit.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Infrastructure
{
    /// <summary>
    /// Represents startup
    /// </summary>
    public static class StartupBase
    {
        private const string AppSectionName = "Application";
        private const string RabbitMqSectionName = "RabbitMq";
        
        #region Utilities

        /// <summary>
        /// Register and init AutoMapper
        /// </summary>
        /// <param name="typeSearcher">Type finder</param>
        private static void InitAutoMapper(IServiceCollection services, ITypeSearcher typeSearcher)
        {
            //find mapper configurations provided by other assemblies
            var mapperConfigurations = typeSearcher.ClassesOfType<IAutoMapperProfile>();

            //create and sort instances of mapper configurations
            var instances = mapperConfigurations
                .Where(mapperConfiguration => PluginExtensions.OnlyInstalledPlugins(mapperConfiguration))
                .Select(mapperConfiguration => (IAutoMapperProfile)Activator.CreateInstance(mapperConfiguration))
                .OrderBy(mapperConfiguration => mapperConfiguration.Order);

            //create AutoMapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                foreach (var instance in instances)
                {
                    cfg.AddProfile(instance.GetType());
                }
            });


            services.AddTransient<IMapper>(_ => config.CreateMapper());

            //register automapper
            AutoMapperConfig.Init(config);
        }

        /// <summary>
        /// Add FluenValidation
        /// </summary>
        /// <param name="mvcCoreBuilder"></param>
        /// <param name="typeSearcher"></param>
        private static void AddFluentValidation(IMvcCoreBuilder mvcCoreBuilder, ITypeSearcher typeSearcher)
        {
            //Add fluentValidation
            mvcCoreBuilder.AddFluentValidation(configuration =>
            {
                var assemblies = typeSearcher.GetAssemblies();
                configuration.RegisterValidatorsFromAssemblies(assemblies);
                configuration.DisableDataAnnotationsValidation = true;
                //implicit/automatic validation of child properties
                configuration.ImplicitlyValidateChildProperties = true;
            });
        }

        /// <summary>
        /// Register type Converters
        /// </summary>
        /// <param name="typeSearcher"></param>
        private static void RegisterTypeConverter(ITypeSearcher typeSearcher)
        {
            //find converters provided by other assemblies
            var converters = typeSearcher.ClassesOfType<ITypeConverter>();

            //create and sort instances of typeConverter 
            var instances = converters
                .Select(converter => (ITypeConverter)Activator.CreateInstance(converter))
                .OrderBy(converter => converter.Order);

            foreach (var item in instances)
            {
                item.Register();
            }
        }

        private static T StartupConfig<T>(this IServiceCollection services, IConfiguration configuration) where T : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var config = new T();
            configuration.Bind(config);
            services.AddSingleton(config);
            return config;
        }

        /// <summary>
        /// Register HttpContextAccessor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        private static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

        /// <summary>
        /// Register extensions plugins/scripts
        /// </summary>
        /// <param name="mvcCoreBuilder"></param>
        /// <param name="configuration"></param>
        private static void RegisterExtensions(IMvcCoreBuilder mvcCoreBuilder, IConfiguration configuration)
        {
            var config = new AppConfig();
            configuration.GetSection(AppSectionName).Bind(config);

            //Load plugins
            PluginManager.Load(mvcCoreBuilder, config);

            //Load CTX sctipts
            RoslynCompiler.Load(mvcCoreBuilder.PartManager, config);
        }

        /// <summary>
        /// Adds services for mediatR
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        private static void AddMediator(this IServiceCollection services, AppTypeSearcher typeSearcher)
        {
            var assemblies = typeSearcher.GetAssemblies().ToArray();
            services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(assemblies));
            //services.AddMediatR(assemblies);
        }

        /// <summary>
        /// Add Mass Transit RabbitMq message broker
        /// </summary>
        /// <param name="services"></param>
        private static void AddMassTransitRabbitMq(IServiceCollection services, IConfiguration configuration, AppTypeSearcher typeSearcher)
        {
            var connectionString = configuration.GetConnectionString(RabbitMqSectionName);

            #region MassTransit

            services.AddMassTransit(x =>
            {
                x.AddConsumers(typeSearcher.GetAssemblies().ToArray());
                x.AddConsumers(typeof(TestDomainEventConsumer).Assembly); // Testing

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

        /// <summary>
        /// Register application 
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration</param>
        private static IMvcCoreBuilder RegisterApplication(IServiceCollection services, IConfiguration configuration)
        {
            //add accessor to HttpContext
            services.AddHttpContextAccessor();

            //add AppConfig configuration parameters
            var config = services.StartupConfig<AppConfig>(configuration.GetSection(AppSectionName));
            //add hosting configuration parameters
            //.StartupConfig<HostingConfig>(configuration.GetSection("Hosting"));
            //add api configuration parameters
            //services.StartupConfig<ApiConfig>(configuration.GetSection("Api"));
            //add grand.web api token config
            services.Configure<WebApiConfig>(configuration.GetSection(nameof(WebApiConfig)));
            //add litedb configuration parameters
            //services.StartupConfig<LiteDbConfig>(configuration.GetSection("LiteDb"));

            //set base application path
            var provider = services.BuildServiceProvider();
            var hostingEnvironment = provider.GetRequiredService<IWebHostEnvironment>();
            var param = configuration["Directory"];
            if (!string.IsNullOrEmpty(param))
            {
                CommonPath.Param = param;
            }

            CommonPath.WebHostEnvironment = hostingEnvironment.WebRootPath;
            CommonPath.BaseDirectory = hostingEnvironment.ContentRootPath;
            CommonHelper.CacheTimeMinutes = config.DefaultCacheTimeMinutes;
            CommonHelper.CookieAuthExpires = config.CookieAuthExpires > 0 ? config.CookieAuthExpires : 24 * 365;

            CommonHelper.IgnoreAcl = config.IgnoreAcl;
            CommonHelper.IgnoreStoreLimitations = config.IgnoreStoreLimitations;

            var mvcCoreBuilder = services.AddMvcCore();

            return mvcCoreBuilder;
        }

        #endregion


        #region Methods


        /// <summary>
        /// Add and configure services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //register application
            var mvcBuilder = RegisterApplication(services, configuration);

            //register extensions 
            RegisterExtensions(mvcBuilder, configuration);

            //find startup configurations provided by other assemblies
            var typeSearcher = new AppTypeSearcher();
            services.AddSingleton<ITypeSearcher>(typeSearcher);

            var startupConfigurations = typeSearcher.ClassesOfType<IStartupApplication>();

            //Register startup
            var instancesBefore = startupConfigurations
                .Where(startup => PluginExtensions.OnlyInstalledPlugins(startup))
                .Select(startup => (IStartupApplication)Activator.CreateInstance(startup))
                .Where(startup => startup.BeforeConfigure)
                .OrderBy(startup => startup.Priority);

            //configure services
            foreach (var instance in instancesBefore)
            {
                instance.ConfigureServices(services, configuration);
            }

            //register mapper configurations
            InitAutoMapper(services, typeSearcher);
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //add fluenvalidation
            AddFluentValidation(mvcBuilder, typeSearcher);

            //Register custom type converters
            RegisterTypeConverter(typeSearcher);

            var config = new AppConfig();
            configuration.GetSection(AppSectionName).Bind(config);

            //add mediator
            AddMediator(services, typeSearcher);

            //Add MassTransit
            AddMassTransitRabbitMq(services, configuration, typeSearcher);

            //Register startup
            var instancesAfter = startupConfigurations
                .Where(startup => PluginExtensions.OnlyInstalledPlugins(startup))
                .Select(startup => (IStartupApplication)Activator.CreateInstance(startup))
                .Where(startup => !startup.BeforeConfigure)
                .OrderBy(startup => startup.Priority);

            //configure services
            foreach (var instance in instancesAfter)
            {
                instance.ConfigureServices(services, configuration);
            }

            //Execute startupbase interface
            ExecuteStartupBase(typeSearcher);
        }

        /// <summary>
        /// Configure HTTP request pipeline
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        /// <param name="webHostEnvironment">WebHostEnvironment</param>
        public static void ConfigureRequestPipeline(IApplicationBuilder application, IWebHostEnvironment webHostEnvironment)
        {
            //find startup configurations provided by other assemblies
            var typeSearcher = new AppTypeSearcher();
            var startupConfigurations = typeSearcher.ClassesOfType<IStartupApplication>();

            //create and sort instances of startup configurations
            var instances = startupConfigurations
                .Where(startup => PluginExtensions.OnlyInstalledPlugins(startup))
                .Select(startup => (IStartupApplication)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Priority);

            //configure request pipeline
            foreach (var instance in instances)
            {
                instance.Configure(application, webHostEnvironment);
            }
        }

        private static void ExecuteStartupBase(AppTypeSearcher typeSearcher)
        {
            var startupBaseConfigurations = typeSearcher.ClassesOfType<IStartupBase>();

            //create and sort instances of startup configurations
            var instances = startupBaseConfigurations
                .Select(startup => (IStartupBase)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Priority);

            //execute
            foreach (var instance in instances)
            {
                instance.Execute();
            }
        }

        #endregion

    }
}
