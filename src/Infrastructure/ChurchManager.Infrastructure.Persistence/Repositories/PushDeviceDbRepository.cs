#region

using ChurchManager.Domain.Features.Communication;
using ChurchManager.Domain.Features.Communication.Repositories;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class PushDeviceDbRepository : GenericRepositoryBase<PushDevice>, IPushDeviceDbRepository
{
    public PushDeviceDbRepository(ChurchManagerDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IList<PushDevice>> PushDevicesForPersonAsync(int personId, CancellationToken ct = default)
    {
            return await Queryable()
                .AsNoTracking()
                .Where(x => x.PersonId == personId)
                .ToListAsync(ct);
        
    }
}