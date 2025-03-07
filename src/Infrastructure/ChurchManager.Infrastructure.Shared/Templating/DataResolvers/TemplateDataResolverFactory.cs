using ChurchManager.Infrastructure.Abstractions.Communication;

namespace ChurchManager.Infrastructure.Shared.Templating.DataResolvers;

public class TemplateDataResolverFactory : ITemplateDataResolverFactory
{
    private readonly IEnumerable<ITemplateDataResolver> _resolvers;

    public TemplateDataResolverFactory(IEnumerable<ITemplateDataResolver> resolvers)
    {
        _resolvers = resolvers;
    }
    
    public ITemplateDataResolver CreateResolver(string templateName)
    {
        var resolver = _resolvers.FirstOrDefault(r => 
            r is TemplateDataResolverBase baseResolver && 
            baseResolver.TemplateName.Equals(templateName, StringComparison.OrdinalIgnoreCase));

        if (resolver == null)
        {
            throw new ArgumentException($"No resolver found for template: {templateName}");
        }

        return resolver;
    }
}