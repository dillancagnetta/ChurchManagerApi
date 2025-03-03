using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Codeboss.Results;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Features.Churches.Services;

public class ChurchGroupService(IGenericDbRepository<ChurchGroup> dbRepository, ILogger<ChurchGroupService> logger) : IChurchGroupService
{
    public async Task<OperationResult> AddChurchGroupAsync(string Name, string Description, int? LeaderPersonId, CancellationToken ct = default)
    {
        try
        {
            await dbRepository.AddAsync(new ChurchGroup
            {
                Name = Name,
                Description = Description,
                LeaderPersonId = LeaderPersonId
            }, ct);

            return new OperationResult(true);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return OperationResult.Fail(e.Message);
        }

    }
}