using ChurchManager.Domain.Shared;
using CodeBoss.Jobs.Model;
using Convey.CQRS.Queries;

namespace ChurchManager.Application.Abstractions.Services;

public interface IJobsService: ICrudServiceAsync<ServiceJob, JobViewModel, EditJobViewModel>
{
    Task<PagedResult<JobViewModel>> BrowseAsync(
        IPagedQuery paging,
        string? searchTerm,
        DateTime? lastSuccessfulRunDateTime = null,
        DateTime? lastRunDateTime = null,
        string? status = null,
        CancellationToken ct = default);
}