using AutoMapper;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class GroupTypeDbRepository  : GenericRepositoryBase<GroupType>, IGroupTypeDbRepository
{
    private readonly IMapper _mapper;

    public GroupTypeDbRepository(ChurchManagerDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }

    public async Task<GroupType> GetCellGroupTypeAsync(CancellationToken ct = default)
    {
       return await Queryable().FirstOrDefaultAsync(x => x.Name == "Cell", ct);
    }
}