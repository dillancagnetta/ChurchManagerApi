﻿using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.Groups.Repositories
{
    public interface IGroupTypeRoleDbRepository: IGenericDbRepository<GroupTypeRole>
    {
        IQueryable<GroupTypeRole> GetByGroupTypeId(int groupTypeId);
    }
}
