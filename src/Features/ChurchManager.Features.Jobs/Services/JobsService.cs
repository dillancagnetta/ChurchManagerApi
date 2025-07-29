using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Application.Features;
using ChurchManager.Domain.Features.Jobs.Specifications;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using CodeBoss.Jobs.Model;
using Convey.CQRS.Queries;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Features.Jobs.Services;

public class JobsService(
    IGenericDbRepository<ServiceJob> dbRepository, 
    ILogger<JobsService> logger,
    IMapper mapper
    ) : CrudServiceAsync<ServiceJob, JobViewModel, EditJobViewModel>(dbRepository, mapper), IJobsService
{
    public async Task<PagedResult<JobViewModel>> BrowseAsync(
        IPagedQuery paging,
        string? searchTerm, DateTime? lastSuccessfulRunDateTime = null, DateTime? lastRunDateTime = null,
        string? status = null, CancellationToken ct = default)
    {
        var spec = new GetJobsSpecification(searchTerm, lastSuccessfulRunDateTime, lastRunDateTime, status);
        var vm = await Repository.BrowseAsync(paging, spec, ct);
        
        return vm;
    }
}