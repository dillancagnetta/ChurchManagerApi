using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.Groups.Repositories;

public interface IGroupTypeDbRepository : IGenericDbRepository<GroupType> 
{
    Task<GroupType?> GetCellGroupTypeAsync(CancellationToken ct = default);
}