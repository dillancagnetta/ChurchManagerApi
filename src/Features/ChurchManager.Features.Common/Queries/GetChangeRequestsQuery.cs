using ChurchManager.Domain.Features.ChangeRequests;
using ChurchManager.Domain.Features.ChangeRequests.Specifications;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Common.Queries;

public record GetChangeRequestsQuery : IRequest<ApiResponse>
{
    public int? ChurchId { get; set; }
    public IList<int>? PersonIds { get; set; }
    public string? Status { get; set; }
}

public class AllChangeRequestsQueryHandler(IReadDbRepository<ChangeRequest> dbRepository) : IRequestHandler<GetChangeRequestsQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetChangeRequestsQuery query, CancellationToken ct)
    {
        var spec = new ChangeRequestListSpecification(query.ChurchId, query.PersonIds, query.Status);
        var vm = await dbRepository.ListAsync(spec, ct);
        
        return new ApiResponse(vm);
    }
}