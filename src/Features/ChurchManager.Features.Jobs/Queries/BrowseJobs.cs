using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Parameters;
using ChurchManager.Domain.Shared;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Jobs.Queries;

public record BrowseJobsQuery : QueryParameter, IRequest<PagedResponse<JobViewModel>>
{
    public string? SearchTerm { get; set; }
    public string? Status { get; set; }
    public DateTime? LastSuccessfulRunDateTime { get; set; }
    public DateTime? LastRunDateTime { get; set; }
};

public class BrowseJobsHandler(IJobsService service) : IRequestHandler<BrowseJobsQuery, PagedResponse<JobViewModel>>
{
    public async Task<PagedResponse<JobViewModel>> Handle(BrowseJobsQuery query, CancellationToken ct)
    {
        var pagedResult = await service.BrowseAsync(
            query, 
            query.SearchTerm,
            query.LastSuccessfulRunDateTime,
            query.LastRunDateTime,
            query.Status, ct);

        return new PagedResponse<JobViewModel>(pagedResult);
    }
}