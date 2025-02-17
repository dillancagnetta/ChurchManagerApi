﻿#region

using ChurchManager.Domain;
using ChurchManager.Domain.Features.Discipleship;
using ChurchManager.Domain.Features.Discipleship.Repositories;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class DiscipleshipDbRepository : GenericRepositoryBase<DiscipleshipStepDefinition>, IDiscipleshipStepDefinitionDbRepository
    {
        public DiscipleshipDbRepository(ChurchManagerDbContext dbContext): base(dbContext)
        {
        }

        public async Task<IEnumerable<dynamic>> DiscipleshipStepInfoForPersonAsync(int personId, int discipleshipStepDefinitionId, CancellationToken ct = default)
        {
            var vm = await Queryable()
                .AsNoTracking()
                .Include(x => x.DiscipleshipProgram)
                .Include(x => x.Steps)
                    .ThenInclude(x => x.Definition)
                .Where(x => x.Steps.Any(step => step.PersonId == personId) &&
                            x.Id == discipleshipStepDefinitionId)
                .SelectMany(x => x.Steps)
                .Select(x => new DiscipleshipStepViewModel
                {
                    CompletionDate = x.CompletionDate,
                    Status = x.Status,
                    IsComplete = x.IsComplete,
                    StepDefinition = new StepDefinitionViewModel { Name = x.Definition.Name, Description = x.Definition.Description }
                })
                .ToListAsync(ct);

            return vm;
        }
    }
}
