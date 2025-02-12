using ChurchManager.Domain.Features.Permissions;
using ChurchManager.Domain.Features.Permissions.Repositories;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Codeboss.Types;

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class EntityPermissionDbRepository : GenericRepositoryBase<EntityPermission>, IEntityPermissionDbRepository
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public EntityPermissionDbRepository(ChurchManagerDbContext dbContext, IDateTimeProvider dateTimeProvider) : base(dbContext)
    {
        _dateTimeProvider = dateTimeProvider;
    }
}