using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Services;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Features.Communication.Services;


/// <summary>
/// Service to build unified communication preferences
/// </summary>
public class CommunicationPreferenceUnifier : ICommunicationPreferenceUnifier
{
    /// <summary>
    /// Creates a unified list of communication preferences combining system defaults and user overrides
    /// </summary>
    /// <param name="systemPreferenceTypes">All system preference types with defaults</param>
    /// <param name="userPreferences">User's overridden preferences</param>
    /// <param name="communicationType">Optional filter by communication type</param>
    /// <param name="category">Optional filter by category</param>
    /// <returns>Unified list with all preference types and their effective values</returns>
    public List<UnifiedCommunicationPreferenceViewModel> BuildUnifiedPreferences(
        IEnumerable<CommunicationPreferenceType> systemPreferenceTypes,
        IEnumerable<CommunicationPreference> userPreferences,
        CommunicationType? communicationType = null,
        string? category = null)
    {
        var userPreferencesLookup = userPreferences
            .ToLookup(p => new
            {
                p.PreferenceTypeId, 
                //CommunicationType = p.CommunicationType.Value, 
                Category = p.Category ?? string.Empty
            });

        var result = new List<UnifiedCommunicationPreferenceViewModel>();

        /*// If we're filtering by communication type
        if (communicationType is not null)
        {
            systemPreferenceTypes = systemPreferenceTypes.Where(t => t.DefaultCommunicationType == communicationType);
        }*/
        
        foreach (var preferenceType in systemPreferenceTypes)
        {
            // If we're filtering by category, handle multiple scenarios
                var categories = !string.IsNullOrEmpty(category)
                    ? new[] { category }
                    : new[] { string.Empty }; // Handle both null/empty categories

                foreach (var cat in categories)
                {
                    var lookupKey = new
                    {
                        PreferenceTypeId = preferenceType.Id,
                        //CommunicationType = preferenceType.DefaultCommunicationType.Value,
                        Category = cat
                    };

                    var userPreference = userPreferencesLookup[lookupKey].FirstOrDefault();

                    if (userPreference != null)
                    {
                        // User has overridden this preference
                        result.Add(new UnifiedCommunicationPreferenceViewModel
                        {
                            PreferenceTypeId = preferenceType.Id,
                            Name = preferenceType.Name,
                            Description = preferenceType.Description,
                            IsSystem = preferenceType.IsSystem,
                            CanOverride = preferenceType.CanOverride,
                            Category = userPreference.Category,
                            CommunicationType = userPreference.CommunicationType,
                            IsEnabled = userPreference.IsEnabled,
                            IsUserOverride = true,
                            UserPreferenceId = userPreference.Id
                        });
                    }
                    else if (preferenceType.CanOverride)
                    {
                        // No user override, use system default (only if user can override - meaning it should be shown)
                        result.Add(new UnifiedCommunicationPreferenceViewModel
                        {
                            PreferenceTypeId = preferenceType.Id,
                            Name = preferenceType.Name,
                            Description = preferenceType.Description,
                            IsSystem = preferenceType.IsSystem,
                            CanOverride = preferenceType.CanOverride,
                            Category = cat == string.Empty ? null : cat,
                            CommunicationType = preferenceType.DefaultCommunicationType,
                            IsEnabled = preferenceType.DefaultNotSetValue,
                            IsUserOverride = false,
                            UserPreferenceId = null
                        });
                    }
                }

            
        }

        // Apply final filters if specified
        if (communicationType != null)
        {
            result = result.Where(r => r.CommunicationType == communicationType.Value).ToList();
        }

        if (!string.IsNullOrEmpty(category))
        {
            result = result.Where(r => r.Category == category).ToList();
        }

        return result.OrderBy(r => r.Name).ThenBy(r => r.CommunicationType).ToList();
    }
    
    // You'll need to implement this based on your CommunicationType structure
    private IEnumerable<CommunicationType> GetAllCommunicationTypes()
    {
        // Example - adjust based on your actual CommunicationType implementation
        return new[]
        {
            CommunicationType.Email,
            CommunicationType.SMS,
            // Add other types as needed
        };
    }
}