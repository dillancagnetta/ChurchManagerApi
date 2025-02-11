﻿#region

using System.Linq.Dynamic.Core;
using ChurchManager.Domain;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Queries;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Infrastructure.Persistence.Extensions;
using CodeBoss.Extensions;
using Codeboss.Results;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class PersonDbRepository : GenericRepositoryBase<Person>, IPersonDbRepository
    {
        private readonly IQueryCache _cache;

        public PersonDbRepository(ChurchManagerDbContext dbContext, IQueryCache cache) : base(dbContext)
        {
            _cache = cache;
        }

        /// <summary>
        /// Finds people who are considered to be good matches based on the query provided.
        /// </summary>
        /// <param name="searchParameters">The search parameters.</param>
        /// <param name="includeDeceased">if set to <c>true</c> [include deceased].</param>
        /// <param name="includes">data table includes</param>
        /// <returns>A IEnumerable of person, ordered by the likelihood they are a good match for the query.</returns>
        public IQueryable<Person> FindPersons(PersonMatchQuery searchParameters, bool includeDeceased = false, params string[] includes)
        {
            // Query by last name, suffix, dob, and gender
            var query = Queryable(includes, includeDeceased)
                .AsNoTracking()
                .Where(p => 
                    p.FullName.FirstName == searchParameters.FirstName &&
                    p.FullName.LastName == searchParameters.LastName);

            if (!searchParameters.Email.IsNullOrEmpty())
            {
                query = query.Where(x => x.Email != null && x.Email.Address == searchParameters.Email);
            }

            return query;
        }

        public IQueryable<Person> Queryable(bool includeDeceased)
        {
            return Queryable(new PersonQueryOptions() {IncludeDeceased = includeDeceased});
        }
        
        public IQueryable<Person> Queryable(string[] includes, bool includeDeceased)
        {
            var options = new PersonQueryOptions() {IncludeDeceased = includeDeceased};
            return Queryable(includes, options);
        }

        public IQueryable<Person> Queryable(PersonQueryOptions personQueryOptions)
        {
            return this.Queryable(null, personQueryOptions);
        }

        public async Task<dynamic> DashboardChurchConnectionStatusBreakdown(int? churchId = null, CancellationToken cancellationToken = default)
        {
            var cacheKey = CacheKeyHelper.CacheKey("DashboardChurchConnectionStatusBreakdown_".ToLower() + (churchId ??= 0));
            
            return await _cache.GetOrSetAsync<dynamic>(cacheKey, async () =>
            {
                var query = Queryable(false).AsNoTracking();
            
                if (churchId.HasValue && churchId.Value > 0)
                {
                    query = query.Where(x => x.ChurchId == churchId.Value);
                }
            
                var connectionStatus = await query.GroupBy(p => p.ConnectionStatus)
                    .Select(g => new { name = g.Key.Value, count = g.Count() })
                    .ToListAsync(cancellationToken);
            
                var gender = await query.GroupBy(p => p.Gender)
                    .Select(g => new { name = g.Key.Value, count = g.Count() })
                    .ToListAsync(cancellationToken);
            
                var age = await query.GroupBy(p => p.AgeClassification)
                    .Select(g => new { name = g.Key.Value, count = g.Count() })
                    .ToListAsync(cancellationToken);
            
                connectionStatus = connectionStatus.OrderBy(x => x.name).ToList();
                gender = gender.OrderBy(x => x.name).ToList();
                age = age.OrderBy(x => x.name).ToList();
            
                return new { connectionStatus, gender, age };
            }, ct: cancellationToken);
        }

        public async Task<OperationResult<Guid?>> UserLoginIdForPersonAsync(int personId, CancellationToken cancellationToken = default)
        {
            var userLoginId = await Queryable()
                .Where(p => p.Id == personId)
                .Select(p => p.UserLoginId)
                .FirstOrDefaultAsync(cancellationToken);

            return new OperationResult<Guid?>(userLoginId.AsGuidOrNull());
        }

        public Task<PersonViewModelBasic> BasicPersonViewModelAsync(int personId, CancellationToken cancellationToken = default)
        {
            return Queryable()
                .AsNoTracking()
                .Where(x => x.Id == personId)
                .Select(x => new PersonViewModelBasic
                {
                    PersonId = x.Id,
                    FullName = x.FullName,
                    Gender = x.Gender,
                    AgeClassification = x.AgeClassification,
                    PhotoUrl = x.PhotoUrl,
                    BirthDate = x.BirthDate,
                    Email = x.Email
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<string> FamilyCode(int personId, CancellationToken cancellationToken = default)
        {
            return Queryable()
                .AsNoTracking()
                    .Include(x => x.Family)
                .FirstOrDefaultAsync(x=> x.Id == personId, cancellationToken)
                .Select(x => x.Family.Code);
        }

        private IQueryable<Person> Queryable(string[] includes, PersonQueryOptions personQueryOptions)
        {
            var qry = base.Queryable(includes);

            if (personQueryOptions.IncludePendingStatus == false)
            {
                qry = qry.Where(x => x.RecordStatus != RecordStatus.Pending);
            }

            if (personQueryOptions.IncludeDeceased)
            {
                qry = qry.Where(p => p.DeceasedStatus != null &&
                                     p.DeceasedStatus.IsDeceased.HasValue &&
                                     p.DeceasedStatus.IsDeceased.Value);
            }

            return qry;
        }
    }
}
