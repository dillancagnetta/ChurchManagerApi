using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Feature = ChurchManager.Domain.Features.Communications;

namespace ChurchManager.Features.Communication.Commands;

public record PreferenceUpdate
{
    public int PreferenceTypeId { get; set; }
    public bool IsEnabled { get; set; }
    public int? UserPreferenceId { get; set; }
    public string CommunicationType { get; set; } = string.Empty;
    public string? Category { get; set; }
}

public record UpdateCommunicationPreferencesCommand : IRequest<ApiResponse>
{
    public int PersonId { get; set; }
    public IEnumerable<PreferenceUpdate> Preferences { get; set; } = new List<PreferenceUpdate>();
}

public class UpdateCommunicationPreferencesHandler(
    IGenericDbRepository<Feature.CommunicationPreference> userPreferencesDb
    ) : IRequestHandler<UpdateCommunicationPreferencesCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(UpdateCommunicationPreferencesCommand command, CancellationToken cancellationToken)
    {
        var updatesLookup = command.Preferences.ToDictionary(x => x.PreferenceTypeId);
        var preferenceTypeIds = updatesLookup.Select(x => x.Key).ToHashSet();
        var userPreferenceIds = command.Preferences.Where(x => x.UserPreferenceId.HasValue).Select(x => x.UserPreferenceId).ToList();
        
        var userPreferences = await userPreferencesDb
            .Queryable()
            .Where(x => userPreferenceIds.Contains(x.Id) && x.PersonId == command.PersonId)
            .ToListAsync(cancellationToken);
        /*
        var systemPreferences = systemPreferencesDb
            .Queryable()
            //.Include(x => x.Preferences)
            .Where(x => preferenceTypeIds.Contains(x.Id))
            //.Where(x => !x.Preferences.Any() || x.Preferences.Any(p => p.PersonId == command.PersonId))
            .ToList();
            */

        bool anyChanges = false;
        foreach (var preferenceTypeId in preferenceTypeIds.ToList())
        {
            var update = updatesLookup[preferenceTypeId];
            // A user preference exists, update it
            if (update is { UserPreferenceId: not null })
            {
                // We should find the user preference
                var userPreference = userPreferences.Single(x => x.Id == update.UserPreferenceId.Value);
                if (userPreference.IsEnabled != update.IsEnabled)
                {
                    userPreference.IsEnabled = update.IsEnabled;
                    anyChanges = true;
                }

                if (userPreference.CommunicationType != update.CommunicationType)
                {
                    userPreference.CommunicationType = update.CommunicationType;
                    anyChanges = true;
                }
            }
            else
            {
                // no user preference exists, create it
                await userPreferencesDb.AddAsync(new Feature.CommunicationPreference
                {
                    IsEnabled = update.IsEnabled,
                    Category = update.Category,
                    CommunicationType = update.CommunicationType,
                    PreferenceTypeId = update.PreferenceTypeId,
                    PersonId = command.PersonId
                }, cancellationToken);
                anyChanges = true;
            }
        }

        if (anyChanges) await userPreferencesDb.SaveChangesAsync(cancellationToken);
     
        return ApiResponse.Success();
    }
}