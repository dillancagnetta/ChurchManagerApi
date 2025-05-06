using ChurchManager.Domain.Features.Churches.Specifications;
using ChurchManager.Domain.Features.Security.Services;
using ChurchManager.Domain.Parameters;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Common;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Feature = ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Features.Communication.Queries;

public record BrowseRecipientsQuery : QueryParameter, IRequest<ApiResponse>
{
    public string Status { get; set; } = Feature.CommunicationRecipientStatus.Pending.Value;
    public int? PersonId { get; set; }
    public int CommunicationId { get; set; }
    public bool? IsSent { get; set; }
}

public class BrowseCommunicationRecipientsHandler(
    IReadDbRepository<Feature.Communication> dbRepository,
    IPermissionContext permissions,
    IAppCurrentUser currentUser) : IRequestHandler<BrowseRecipientsQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(BrowseRecipientsQuery query, CancellationToken ct)
    {
        var spec = new BrowseRecipientsSpecification(query, query.CommunicationId, query.Status, query.PersonId);
        
        var results = await dbRepository.BrowseAsync<CommunicationRecipientViewModel>(query, spec, ct);
            
        return new PagedResponse<CommunicationRecipientViewModel>(results); 
    }
}