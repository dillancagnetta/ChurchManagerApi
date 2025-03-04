using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Shared;
using Codeboss.Results;

namespace ChurchManager.Application.Abstractions.Services;

public interface IChurchGroupService: ICrudServiceAsync<ChurchGroup, ChurchGroupViewModel>
{
   Task<OperationResult>  AddChurchGroupAsync(string Name, string Description, int? LeaderPersonId, CancellationToken ct = default);
}