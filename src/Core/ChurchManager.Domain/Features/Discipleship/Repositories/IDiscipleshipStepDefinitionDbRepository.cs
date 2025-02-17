﻿using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.Discipleship.Repositories
{
    public interface IDiscipleshipStepDefinitionDbRepository : IGenericDbRepository<DiscipleshipStepDefinition>
    {
        Task<IEnumerable<dynamic>> DiscipleshipStepInfoForPersonAsync(int personId, int discipleshipStepDefinitionId, CancellationToken ct = default);
    }
}
