using ChurchManager.Infrastructure.Abstractions.Persistence;
using CodeBoss.Jobs.Model;

namespace ChurchManager.Domain.Features.Jobs.Repositories;

public interface IJobsDbRepository : IGenericDbRepository<ServiceJob>
{
    
}