using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Codeboss.Results;

namespace ChurchManager.Domain.Features.People.Repositories;

public interface IFamilyDbRepository: IGenericDbRepository<Family>
{
    Task<FamilyCodeValidationViewModel> ValidateFamilyCodeAsync(string familyCode, CancellationToken ct = default);
    Task<OperationResult<FamilyViewModel>> FamilyByCodeAsync(string familyCode, string emailAddress, CancellationToken ct = default);
    Task<IList<int>> PersonIdsOfFamilyMembersAsync(int familyId, CancellationToken ct = default);
}