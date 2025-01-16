using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.Jobs.Abstractions;
using CodeBoss.Jobs.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManager.Infrastructure.Shared.Jobs;

public class CmServiceJobRepository(IDbContextFactory<ChurchManagerDbContext> dbContextFactory) : IServiceJobRepository
{
    public async Task<IEnumerable<ServiceJob>> GetActiveJobsAsync(CancellationToken ct = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);
        return await dbContext.Set<ServiceJob>()
            .Where(x => x.IsActive)
            .ToListAsync(ct);
    }

    private IChurchManagerDbContext CreateDbContext(IServiceScope scope)
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<IChurchManagerDbContext>();
        return dbContext;
    }

    public async Task AddOrUpdateAsync(ServiceJob job, CancellationToken ct = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);
        // Edit
        if (await dbContext.Set<ServiceJob>().AnyAsync(x => x.Id == job.Id, cancellationToken: ct))
        {
            throw new NotImplementedException();
        }
        else
        {
            await dbContext.Set<ServiceJob>().AddAsync(job, ct);
        }

        await dbContext.SaveChangesAsync(ct);
    }

    public  async Task DeleteAsync(int serviceJobId, CancellationToken ct = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);
        var job = await GetByIdAsync(dbContext, serviceJobId, ct);
        if (job != null)
        {
            job.IsActive = false;
            await dbContext.SaveChangesAsync(ct);
        }
    }

    public async Task<ServiceJob> GetByIdAsync(int serviceJobId, CancellationToken ct = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);
        return await dbContext.Set<ServiceJob>()
            .FirstOrDefaultAsync(x => x.Id == serviceJobId, cancellationToken: ct);
    }
    
    public async Task<ServiceJob> GetByIdAsync(IChurchManagerDbContext dbContext, int serviceJobId, CancellationToken ct = default)
    {
        return await dbContext.Set<ServiceJob>()
            .FirstOrDefaultAsync(x => x.Id == serviceJobId, cancellationToken: ct);
    }

    public async Task UpdateLastStatusMessageAsync(int serviceJobId, string statusMessage, CancellationToken ct = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);
        var job = await GetByIdAsync(dbContext, serviceJobId, ct);
        if (job != null)
        {
            job.LastStatusMessage = statusMessage;
            
            if (job.EnableHistory)
            {
                job.ServiceJobHistory.Add(new ServiceJobHistory
                {
                    ServiceJob = job,
                    ServiceJobId = job.Id,
                    StartDateTime = DateTime.UtcNow.AddMinutes(-1), // TODO: Use listener to get time
                    StopDateTime = DateTime.UtcNow,
                    Status = job.LastStatus,
                    StatusMessage = job.LastStatusMessage
                });
            }
            
            await dbContext.SaveChangesAsync(ct);
        }
    }
    
    public async Task UpdateStatusMessagesAsync(int serviceJobId, string message, string status, CancellationToken ct = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);
        var job = await GetByIdAsync(dbContext, serviceJobId, ct);
        if (job != null)
        {
            job.LastStatusMessage = message;
            job.LastStatus = status;

            if (job.EnableHistory)
            {
                job.ServiceJobHistory.Add(new ServiceJobHistory
                {
                    ServiceJob = job,
                    ServiceJobId = job.Id,
                    StartDateTime = DateTime.UtcNow.AddMinutes(-1), // TODO: Use listener to get time
                    StopDateTime = DateTime.UtcNow,
                    Status = job.LastStatus,
                    StatusMessage = job.LastStatusMessage
                });
            }
            
            await dbContext.SaveChangesAsync(ct);
        }
    }

    public async Task ClearStatusesAsync(ServiceJob serviceJob, CancellationToken ct = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct);
        var job = await GetByIdAsync(dbContext, serviceJob.Id, ct);
        if (job != null)
        {
            job.LastStatusMessage = string.Empty;
            job.LastStatus = string.Empty;
            await dbContext.SaveChangesAsync(ct);
        }
    }

    public Task UpdateLastStatusMessageAsync(ServiceJob serviceJob, string statusMessage, CancellationToken ct = default)
    {
        return UpdateLastStatusMessageAsync(serviceJob.Id, statusMessage, ct);
    }
}