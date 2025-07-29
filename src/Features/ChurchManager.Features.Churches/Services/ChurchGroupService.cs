using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Application.Features;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Codeboss.Results;
using Microsoft.Extensions.Logging;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Features.Churches.Services;

public class ChurchGroupService(
    IGenericDbRepository<ChurchGroup> dbRepository, 
    ILogger<ChurchGroupService> logger,
    IMapper mapper) : CrudServiceAsync<ChurchGroup, ChurchGroupViewModel, EditChurchGroupModel>(dbRepository, mapper), IChurchGroupService
{
    public async Task<OperationResult> AddChurchGroupAsync(string name, string description, int? leaderPersonId, CancellationToken ct = default)
    {
        try
        {
            await Repository.AddAsync(new ChurchGroup
            {
                Name = name,
                Description = description,
                LeaderPersonId = leaderPersonId
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