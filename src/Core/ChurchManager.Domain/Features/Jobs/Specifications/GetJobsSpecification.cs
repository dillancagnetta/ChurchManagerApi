using Ardalis.Specification;
using ChurchManager.Domain.Features.Jobs.Extensions;
using ChurchManager.Domain.Shared;
using CodeBoss.Extensions;
using CodeBoss.Jobs.Model;

namespace ChurchManager.Domain.Features.Jobs.Specifications;

public class GetJobsSpecification: Specification<ServiceJob, JobViewModel>
{
    public GetJobsSpecification(
        string? searchTerm,
        DateTime? lastSuccessfulRunDateTime = null,
        DateTime? lastRunDateTime = null,
        string? status = null)
    {
        Query.AsNoTracking();
        Query.Include(x => x.HistoryCount);
        
        if (!searchTerm.IsNullOrEmpty())
        {
            Query.Where(j => j.Name.Contains(searchTerm) || j.Description.Contains(searchTerm));
        }
        
        if (lastSuccessfulRunDateTime.HasValue)
        {
            Query.Where(j => j.LastSuccessfulRunDateTime >= lastSuccessfulRunDateTime);
        }
        
        if (lastRunDateTime.HasValue)
        {
            Query.Where(j => j.LastRunDateTime >= lastRunDateTime);
        }
        
        if (!status.IsNullOrEmpty())
        {
            Query.Where(j => j.LastStatus.Contains(searchTerm));
        }
        
        Query.OrderByDescending(j => j.LastRunDateTime);

        Query.Select(j => j.ToViewModel());
    }
}