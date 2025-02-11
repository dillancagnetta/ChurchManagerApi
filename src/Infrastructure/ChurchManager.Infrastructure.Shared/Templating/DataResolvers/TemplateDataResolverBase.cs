using System.Reflection;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Abstractions.Communication;
using ChurchManager.Infrastructure.Shared.Templating.DataResolvers;

namespace ChurchManager.Infrastructure.Shared.Templating;

// Base abstract class with common resolution logic
public abstract class TemplateDataResolverBase(IPersonDbRepository personDb) : ITemplateDataResolver
{
    public virtual string TemplateName
    {
        get
        {
            var attribute = GetType().GetCustomAttribute<TemplateNameAttribute>();
            if (attribute == null)
            {
                throw new InvalidOperationException($"Template resolver {GetType().Name} must have TemplateName attribute");
            }
            return attribute.Name;
        }
    }
    
    public async Task<Dictionary<string, string>> ResolveDataAsync(int personId, IDictionary<string, object> additionalData = null, CancellationToken ct = default)
    {
        // Resolve base data common to all templates
        var baseData = await ResolveBaseDataAsync(personId, ct);
        
        // Resolve template-specific data
        var specificData = await ResolveTemplateSpecificDataAsync(personId, additionalData, ct);
        
        // Combine and return
        return baseData.Concat(specificData).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
    
    protected virtual async Task<Dictionary<string, string>> ResolveBaseDataAsync(int personId, CancellationToken ct = default)
    {
        var person = await personDb.BasicPersonViewModelAsync(personId, ct);
        
        return new Dictionary<string, string>
        {
            ["Title"] = person.FullName.Title,
            ["FirstName"] = person.FullName.FirstName,
            ["LastName"] = person.FullName.LastName,
            ["CreationDate"] = DateTime.UtcNow.ToShortDateString()
        };
    }
    
    protected abstract Task<Dictionary<string, string>> ResolveTemplateSpecificDataAsync(
        int personId,
        IDictionary<string, object> additionalData,
        CancellationToken ct = default);
}