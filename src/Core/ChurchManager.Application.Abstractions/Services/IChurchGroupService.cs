using Codeboss.Results;

namespace ChurchManager.Application.Abstractions.Services;

public interface IChurchGroupService
{
   Task<OperationResult>  AddChurchGroupAsync(string Name, string Description, int? LeaderPersonId, CancellationToken ct = default);
}