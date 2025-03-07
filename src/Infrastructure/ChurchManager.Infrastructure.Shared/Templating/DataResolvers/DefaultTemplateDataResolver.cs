using ChurchManager.Domain.Features.People.Repositories;

namespace ChurchManager.Infrastructure.Shared.Templating.DataResolvers;

[TemplateName("DefaultNoTemplate")]
public class DefaultTemplateDataResolver(IPersonDbRepository personDb) : TemplateDataResolverBase(personDb)
{
    protected override Task<Dictionary<string, string>> ResolveTemplateSpecificDataAsync(int personId, IDictionary<string, object> additionalData, CancellationToken ct = default)
    {
        return Task.FromResult(new Dictionary<string, string>());
    }
}