using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Codeboss.Results;

namespace ChurchManager.Domain.Features.People.Repositories;

public interface IFamilyDbRepository: IGenericDbRepository<Family>
{
    Task<FamilyCodeValidationViewModel> ValidateFamilyCodeAsync(string familyCode, CancellationToken ct = default);
    Task<OperationResult<Family>> FamilyByCodeAsync(string familyCode, CancellationToken ct = default);
}