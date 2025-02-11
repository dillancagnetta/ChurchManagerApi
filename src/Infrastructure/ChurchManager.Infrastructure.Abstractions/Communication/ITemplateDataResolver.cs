using Codeboss.Types;

namespace ChurchManager.Infrastructure.Abstractions.Communication;

// Base interface for template data resolution
public interface ITemplateDataResolver
{
    Task<Dictionary<string, string>> ResolveDataAsync(int personId, IDictionary<string, object> additionalData = null, CancellationToken ct = default);
}