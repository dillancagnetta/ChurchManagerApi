using AutoMapper;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Persistence.Contexts;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class UserLoginDbRepository : GenericRepositoryBase<UserLogin>,  IUserLoginDbRepository
{
    private readonly IMapper _mapper;
    
    public UserLoginDbRepository(ChurchManagerDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }

    public Task<Guid?> UserLoginIdForAsync(Person person, CancellationToken token = default)
    {
        return UserLoginIdForAsync(person.Id, token);
    }

    public async Task<Guid?> UserLoginIdForAsync(int personId, CancellationToken token = default)
    {
        return await Queryable()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PersonId == personId, cancellationToken: token)
            .Select(x => x.Id);;
    }
}