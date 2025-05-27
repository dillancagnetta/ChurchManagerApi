using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using ChurchManager.Domain.Shared;
using Codeboss.Results;

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class FamilyDbRepository : GenericRepositoryBase<Family>, IFamilyDbRepository
{
    public FamilyDbRepository(ChurchManagerDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<FamilyCodeValidationViewModel> ValidateFamilyCodeAsync(string familyCode, CancellationToken ct = default)
    {
        var isFound = await Queryable()
            .AsNoTracking()
            .Include(x => x.FamilyMembers)
            .SingleOrDefaultAsync(x => x.Code == familyCode, ct);
        
        if(isFound is null) return new FamilyCodeValidationViewModel { IsValid = false };
        
        return new FamilyCodeValidationViewModel
        {
            IsValid = true,
            FamilyMembers = isFound.FamilyMembers.Select(x => new PersonViewModelBasic
            {
                PersonId   = x.Id,
                Gender = x.Gender.Value,
                FirstName = x.FullName!.FirstName,
                LastName = x.FullName!.LastName,
                AgeClassification = x.AgeClassification,
                Age = x.BirthDate?.Age,
                PhotoUrl = x.PhotoUrl
            })
        };
    }

    public async Task<OperationResult<Family>> FamilyByCodeAsync(string familyCode, CancellationToken ct)
    {
        try
        {
            familyCode = familyCode.Trim().ToUpperInvariant();
            var family = await Queryable()
                .AsNoTracking()
                .Include(x => x.FamilyMembers)
                .SingleOrDefaultAsync(x => x.Code == familyCode, ct);

            return new OperationResult<Family>(family != null, family);
        }
        catch (Exception e)
        {
            return OperationResult<Family>.Fail(e.Message);
        }
    }
}