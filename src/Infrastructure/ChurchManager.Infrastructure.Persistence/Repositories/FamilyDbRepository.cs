using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

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
                //Gender = x.Gender,
                FirstName = x.FullName.FirstName,
                LastName = x.FullName.LastName,
                AgeClassification = x.AgeClassification,
                //Age = x.BirthDate.Age,
                //PhotoUrl = x.PhotoUrl
            })
        };
    }
}