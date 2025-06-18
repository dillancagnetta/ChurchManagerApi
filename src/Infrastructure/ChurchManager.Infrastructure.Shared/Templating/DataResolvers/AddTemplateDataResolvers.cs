using System.Reflection;
using ChurchManager.Infrastructure.Abstractions.Communication;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Infrastructure.Shared.Templating.DataResolvers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateDataResolvers(this IServiceCollection services, Assembly assembly = null)
    {
        assembly ??= Assembly.GetExecutingAssembly();

        // Find all classes that inherit from TemplateDataResolverBase and have TemplateNameAttribute
        var resolverTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && 
                        typeof(TemplateDataResolverBase).IsAssignableFrom(t) &&
                        t.GetCustomAttribute<TemplateNameAttribute>() != null);

        foreach (var resolverType in resolverTypes)
        {
            services.AddScoped(typeof(ITemplateDataResolver), resolverType);
        }

        services.AddScoped<ITemplateDataResolverFactory, TemplateDataResolverFactory>();
        
        return services;
    }
}