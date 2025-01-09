using ChurchManager.Domain.Common;
using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.People.Repositories;

public interface IUserLoginDbRepository : IGenericDbRepository<UserLogin>
{
    Task<Guid?> UserLoginIdForAsync(Person person, CancellationToken token = default);
    Task<Guid?> UserLoginIdForAsync(int personId, CancellationToken token = default);
}