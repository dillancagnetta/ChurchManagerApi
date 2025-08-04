using System.Reflection;
using AutoMapper;
using ChurchManager.Infrastructure.Abstractions;
using ChurchManager.Infrastructure.Abstractions.Configuration;
using ChurchManager.Infrastructure.Mapper;
using ChurchManager.Infrastructure.Plugins;
using ChurchManager.Infrastructure.Shared.Tests;
using ChurchManager.Infrastructure.TypeConverters;
using ChurchManager.Infrastructure.TypeSearcher;
using ChurchManager.SharedKernel;
using ChurchManager.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;
using Wolverine.RabbitMQ;

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
                .Select(mapperConfiguration => (IAutoMapperProfile)Activator.CreateInstance(mapperConfiguration)!)
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
        /// Register type Converters
        /// </summary>
        /// <param name="typeSearcher"></param>
        private static void RegisterTypeConverter(ITypeSearcher typeSearcher)
        {
            //find converters provided by other assemblies
            var converters = typeSearcher.ClassesOfType<ITypeConverter>();

            //create and sort instances of typeConverter 
            var instances = converters
                .Select(converter => (ITypeConverter)Activator.CreateInstance(converter)!)
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
        /// <param name="config"></param>
        private static void RegisterExtensions(IMvcCoreBuilder mvcCoreBuilder, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            //Load plugins
            PluginManager.Load(mvcCoreBuilder, configuration, hostEnvironment);

            //Load CTX sctipts
            //RoslynCompiler.Load(mvcCoreBuilder.PartManager, config);
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

        #region Wolverine
        
        /// <summary>
        /// Add Mass Transit RabbitMq message broker
        /// </summary>
        /// <param name="services"></param>
        private static void AddWolverineRabbitMq(
            IServiceCollection services, IConfiguration configuration, AppTypeSearcher typeSearcher, AppConfig config)
        {
            
            services.AddWolverine(x =>
            {
                // Load handlers from multiple assemblies
                var handlerTypes = typeSearcher.GetAssemblies()
                    .SelectMany(a => a.GetTypes()
                        .Where(t => typeof(IDomainEventHandler)
                        .IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false }));
                foreach (var type in handlerTypes)
                {
                    x.Discovery.IncludeType(type);
                    //x.Discovery.IncludeAssembly(assembly);
                }
                x.Discovery.IncludeAssembly(typeof(TestDomainEventConsumer).Assembly); // Testing
                
                if (config.RabbitMqEnabled)
                {
                    var connectionString = configuration.GetConnectionString(RabbitMqSectionName) 
                        ?? throw new ArgumentNullException(nameof(RabbitMqSectionName));
      
                    x.UseRabbitMq(cfg =>
                    {
                        cfg.Uri = new Uri(connectionString);
                    })
                    .AutoProvision()
                    .UseConventionalRouting(r =>
                    {
                        // Customize the naming convention for the outgoing exchanges
                        r.ExchangeNameForSending(type => type.Name);

                        // Customize the naming convention for incoming queues
                        r.QueueNameForListener(type => type.FullName!
                            .Replace("ChurchManager.Domain.Features.", "")
                            .Replace("ChurchManager.Infrastructure.Shared.", "")
                        );
                    });
                }
                // Setup in-memory transport/queue
            });
        }
        #endregion

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

            services.Configure<WebApiConfig>(configuration.GetSection(nameof(WebApiConfig)));

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
            
            PluginPaths.Initialize(CommonPath.InstalledPluginsFilePath);

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
            // ApplicationConfig
            var config = new AppConfig();
            configuration.GetSection(AppSectionName).Bind(config);
            services.Configure<AppConfig>(configuration.GetSection(AppSectionName));
            
            Console.WriteLine($"[AppConfig] RabbitMqEnabled: {config.RabbitMqEnabled}");
            Console.WriteLine($"[AppConfig] EmailSendingEnabled: {config.EmailSendingEnabled}");
            Console.WriteLine($"[AppConfig] SMSSendingEnabled: {config.SMSSendingEnabled}");
            Console.WriteLine($"[AppConfig] AWSParameterStoreEnabled: {config.AWSParameterStoreEnabled}");
            
            //find startup configurations provided by other assemblies
            var typeSearcher = new AppTypeSearcher();
            services.AddSingleton<ITypeSearcher>(typeSearcher);
            
            var provider = services.BuildServiceProvider();
            var hostingEnvironment = provider.GetRequiredService<IWebHostEnvironment>();
            
            //register application
            var mvcBuilder = RegisterApplication(services, configuration);
            
            //register extensions 
            RegisterExtensions(mvcBuilder, configuration, hostingEnvironment);
            
            var startupConfigurations = typeSearcher.ClassesOfType<IStartupApplication>();

            //Register startup
            var instancesBefore = startupConfigurations
                .Where(startup => PluginExtensions.OnlyInstalledPlugins(startup))
                .Select(startup => (IStartupApplication)Activator.CreateInstance(startup)!)
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
            
            //Register custom type converters
            RegisterTypeConverter(typeSearcher);

            //add mediator
            AddMediator(services, typeSearcher);

            //Add MassTransit
            AddWolverineRabbitMq(services, configuration, typeSearcher, config);

            //Register startup
            var instancesAfter = startupConfigurations
                .Where(startup => PluginExtensions.OnlyInstalledPlugins(startup))
                .Select(startup => (IStartupApplication)Activator.CreateInstance(startup)!)
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
                .Select(startup => (IStartupApplication)Activator.CreateInstance(startup)!)
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
                .Select(startup => (IStartupBase)Activator.CreateInstance(startup)!)
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
