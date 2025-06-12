using ChurchManager.Domain.Features.ChangeRequests;
using ChurchManager.Domain.Features.ChangeRequests.Specifications;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Parameters;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using CodeBoss.Extensions;
using MediatR;

namespace ChurchManager.Features.Common.Queries;

public record BrowseChangeRequestsQuery: QueryParameter, IRequest<PagedResponse<ChangeRequestViewModel>>
{
    public int? ChurchId { get; set; }
    public IList<int>? PersonIds { get; set; }
    public string? Status { get; set; }
    public string? EntityType { get; set; } // Person, Church, etc.
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}

public class BrowseChangeRequestsHandler(
    IReadDbRepository<ChangeRequest> dbRepository,
    IPersonDbRepository personDb
) : IRequestHandler<BrowseChangeRequestsQuery, PagedResponse<ChangeRequestViewModel>>
{
    public async Task<PagedResponse<ChangeRequestViewModel>> Handle(BrowseChangeRequestsQuery query, CancellationToken ct)
    {
       var spec = new BrowseChangeRequestSpecification(query, query.ChurchId, query.PersonIds, query.Status, query.From, query.To);
       
       var pagedResult = await dbRepository.BrowseAsync(query, spec, ct);

       // Not filtering by people
       if (query.EntityType != "Person") return new PagedResponse<ChangeRequestViewModel>(pagedResult);
       
       /*
        * Augment with person details
        */
       // Get all person change requests
       var personChangeRequests = pagedResult.Items.Where(x => x.EntityType == "Person").ToList();
       var personIds = personChangeRequests.Select(x => x.EntityId!.Value).ToHashSet();
        
       var basicPersonLookup = (await personDb.BasicPersonsViewModelAsync(personIds.ToList(), ct))
           .ToDictionary(x => x!.PersonId);

       foreach (var vm in pagedResult.Items)
       {
           vm.PersonEntity = basicPersonLookup[vm.EntityId!.Value];
       }

       return new PagedResponse<ChangeRequestViewModel>(pagedResult);
    }
}