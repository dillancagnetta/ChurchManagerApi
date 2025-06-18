using ChurchManager.Domain.Features.ChangeRequests;
using ChurchManager.Domain.Features.ChangeRequests.Specifications;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using CodeBoss.Extensions;
using MediatR;

namespace ChurchManager.Features.Common.Queries;

public record GetChangeRequestsQuery : IRequest<ApiResponse>
{
    public int? ChurchId { get; set; }
    public IList<int>? PersonIds { get; set; }
    public string? Status { get; set; }
}

public class AllChangeRequestsQueryHandler(
    IReadDbRepository<ChangeRequest> dbRepository,
    IPersonDbRepository personDb
    ) : IRequestHandler<GetChangeRequestsQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetChangeRequestsQuery query, CancellationToken ct)
    {
        var spec = new ChangeRequestListSpecification(query.ChurchId, query.PersonIds, query.Status);
        var vms = await dbRepository.ListAsync(spec, ct);

        // Not filtering by people
        if (query.PersonIds.IsNullOrEmpty()) return new ApiResponse(vms);
        
       /*
        * Augment with person details
        */
        // Get all person change requests
        var personChangeRequests = vms.Where(x => x.EntityType == "Person").ToList();
        var personIds = personChangeRequests.Select(x => x.EntityId!.Value).ToHashSet();
        
        var basicPersonLookup = (await personDb.BasicPersonsViewModelAsync(personIds.ToList(), ct))
            .ToDictionary(x => x!.PersonId);

        foreach (var vm in vms)
        {
            vm.PersonEntity = basicPersonLookup[vm.EntityId!.Value];
        }
        
        return new ApiResponse(vms);
    }
}