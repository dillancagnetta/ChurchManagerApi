using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Services;
using ChurchManager.Domain.Features.Communications.Specifications;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Common;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Feature = ChurchManager.Domain.Features.Communications;

namespace ChurchManager.Features.Communication.Queries.Preferences;

public record GetCommunicationPreferencesQuery : IRequest<ApiResponse>
{
    public string? Name { get; set; }
    public CommunicationType? CommunicationType { get; set; }
    public int? PersonId { get; set; }
}

public class CommunicationPreferencesQueryHandler(
    IReadDbRepository<Feature.CommunicationPreferenceType> dbRepository,
    IReadDbRepository<Feature.CommunicationPreference> preferencesDb,
    ICommunicationPreferenceUnifier unifier,
    IAppCurrentUser currentUser) : IRequestHandler<GetCommunicationPreferencesQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetCommunicationPreferencesQuery query, CancellationToken ct)
    {
        var spec = new CommunicationPreferencesQuerySpecification(query.CommunicationType, query.PersonId);
        var userPreferences = await preferencesDb.ListAsync(spec, ct);

        var preferenceTypesSpec = new CommunicationPreferencesTypesSpecification();
        var systemPreferences = await dbRepository.ListAsync(preferenceTypesSpec, ct);
        
        var unifiedVm = unifier.BuildUnifiedPreferences(
            systemPreferences,
            userPreferences
        );
        
        return new ApiResponse(unifiedVm);
    }
}