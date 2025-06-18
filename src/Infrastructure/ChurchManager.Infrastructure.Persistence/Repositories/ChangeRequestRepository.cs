using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.ChangeRequests;
using ChurchManager.Domain.Features.ChangeRequests.Repositories;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class ChangeRequestRepository(ChurchManagerDbContext dbContext) : GenericRepositoryBase<ChangeRequest>(dbContext), IChangeRequestRepository
{
    public async Task<IList<ChangeRequest>> AllPendingRequestsAsync(bool includeProperties = false, CancellationToken ct = default)
    {
        var query = Queryable().AsNoTracking();

        if (includeProperties)
        {
            query.Include(cr => cr.Properties);
        }
        
        return await query
            .Where(cr => cr.Status == ApprovalStatus.PendingApproval)
            .OrderBy(cr => cr.RequestedDate)
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<IList<ChangeRequest>> RequestsForEntityAsync(string entityType, int entityId,
        CancellationToken ct = default)
    {
        var query = Queryable();

        query.Include(cr => cr.Properties);
        
        return await query
            .Where(cr => cr.Status == ApprovalStatus.PendingApproval)
            .Where(cr => cr.Properties.Any(p => p.EntityType == entityType && p.EntityId == entityId))
            .OrderBy(cr => cr.RequestedDate)
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<IList<ChangeRequest>> RequestsByPersonAsync(int personId, CancellationToken ct = default)
    {
        var query = Queryable();

        query.Include(cr => cr.Properties);
        
        return await query
            .Where(cr => cr.Status == ApprovalStatus.PendingApproval)
            .Where(cr => cr.Properties.Any(p => p.EntityId == personId))
            .OrderBy(cr => cr.RequestedDate)
            .ToListAsync(cancellationToken: ct);
    }
}