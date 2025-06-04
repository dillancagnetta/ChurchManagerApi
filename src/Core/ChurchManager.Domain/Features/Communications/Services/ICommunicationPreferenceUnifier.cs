using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Communications.Services;

public interface ICommunicationPreferenceUnifier
{
    /// <summary>
    /// Creates a unified list of communication preferences combining system defaults and user overrides
    /// </summary>
    /// <param name="systemPreferenceTypes">All system preference types with defaults</param>
    /// <param name="userPreferences">User's overridden preferences</param>
    /// <param name="communicationType">Optional filter by communication type</param>
    /// <param name="category">Optional filter by category</param>
    /// <returns>Unified list with all preference types and their effective values</returns>
    List<UnifiedCommunicationPreferenceViewModel> BuildUnifiedPreferences(
        IEnumerable<CommunicationPreferenceType> systemPreferenceTypes,
        IEnumerable<CommunicationPreference> userPreferences,
        CommunicationType? communicationType = null,
        string? category = null);
}