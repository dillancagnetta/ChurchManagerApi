using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.People.Repositories;

public interface IFamilyDbRepository: IGenericDbRepository<Family>
{
    Task<FamilyCodeValidationViewModel> ValidateFamilyCodeAsync(string familyCode, CancellationToken ct = default);
}