using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Shared.Templating.DataResolvers;

namespace ChurchManager.Infrastructure.Shared.Templating;

[TemplateName("SMSTemplateTest")]
public class SMSTemplateTestTemplateDataResolver(IPersonDbRepository personDb) : TemplateDataResolverBase(personDb)
{
    protected override Task<Dictionary<string, string>> ResolveTemplateSpecificDataAsync(int personId, IDictionary<string, object> additionalData, CancellationToken ct = default)
    {
        return Task.FromResult(new Dictionary<string, string>());
    }
}