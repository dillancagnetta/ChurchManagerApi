using ChurchManager.Domain.Features.Churches.Specifications;
using ChurchManager.Domain.Features.Security;
using ChurchManager.Domain.Features.Security.Services;
using ChurchManager.Domain.Parameters;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Common;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Feature = ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Features.Communication.Queries;

public record BrowseCommunicationsQuery : SearchTermQueryParameter, IRequest<ApiResponse>
{
    public string CommunicationStatus { get; set; } = Feature.CommunicationStatus.PendingApproval.Value;
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public int? RecipientPersonId { get; set; }
    public int? CommunicationTemplateId { get; set; }
    public IEnumerable<string> CommunicationTypes { get; set; } = [];
}

public class BrowseCommunicationsHandler(
    IReadDbRepository<Feature.Communication> dbRepository,
    IPermissionContext permissions,
    IAppCurrentUser currentUser) : IRequestHandler<BrowseCommunicationsQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(BrowseCommunicationsQuery query, CancellationToken ct)
    {
        var allowedIds = await permissions.GetAllowedIdsAsync<Feature.Communication>(
            userLoginId: Guid.Parse((string)currentUser.Id), PermissionAction.View, ct);

        var spec = new BrowseCommunicationsSpecification(
            query,
            query.CommunicationTypes,
            query.CommunicationStatus,
            query.SearchTerm,
            query.From, query.To,
            query.RecipientPersonId,
            query.CommunicationTemplateId,
            allowedIds: allowedIds
        );

        var results = await dbRepository.BrowseAsync<CommunicationViewModel>(query, spec, ct);

        return new PagedResponse<CommunicationViewModel>(results);
    }
}