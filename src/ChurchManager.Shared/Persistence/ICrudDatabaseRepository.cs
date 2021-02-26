﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codeboss.Types;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Shared.Persistence
{
    public interface ICrudDatabaseRepository<T> where T : class, IAggregateRoot<int>
    {
        DbContext DbContext { get; }
        IQueryable<T> Queryable();
        IQueryable<T> Queryable(ISpecification<T> specification);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<int> AddAsync(T item);
        Task AddRangeAsync(IEnumerable<T> items);
        Task UpdateAsync(int id, T sourceItem);
        Task Delete(int id);
        Task<int> SaveChangesAsync();
    }
}