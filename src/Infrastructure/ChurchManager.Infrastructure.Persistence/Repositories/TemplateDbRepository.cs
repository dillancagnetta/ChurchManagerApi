using ChurchManager.Domain.Features.Communication.Repositories;
using ChurchManager.Domain.Features.Communications;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class TemplateDbRepository  : GenericRepositoryBase<CommunicationTemplate>, ITemplateRepository
{
    public TemplateDbRepository(ChurchManagerDbContext dbContext) : base(dbContext)
    {
    }

    public Task<CommunicationTemplate> TemplateByNameAsync(string name, CancellationToken ct = default)
    {
        return Queryable()
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Name == name, cancellationToken: ct);
    }
}