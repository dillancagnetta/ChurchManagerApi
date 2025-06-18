using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Security.Repositories;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Codeboss.Types;

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class UserLoginRoleDbRepository : GenericRepositoryBase<UserLoginRole>, IUserLoginRoleDbRepository
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public UserLoginRoleDbRepository(ChurchManagerDbContext dbContext, IDateTimeProvider dateTimeProvider) : base(dbContext)
    {
        _dateTimeProvider = dateTimeProvider;
    }
}