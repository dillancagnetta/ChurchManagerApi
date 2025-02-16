using ChurchManager.Domain.Common;
using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.Security.Repositories;

public interface IUserLoginRoleDbRepository: IGenericDbRepository<UserLoginRole>
{
    
}

public interface IEntityPermissionDbRepository : IGenericDbRepository<EntityPermission>;
