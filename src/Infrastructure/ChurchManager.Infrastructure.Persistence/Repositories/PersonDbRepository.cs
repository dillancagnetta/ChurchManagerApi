#region

using System.Linq.Dynamic.Core;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Extensions;
using ChurchManager.Domain.Features.People.Queries;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Infrastructure.Persistence.Contexts;
using ChurchManager.Infrastructure.Persistence.Extensions;
using CodeBoss.Extensions;
using Codeboss.Results;
using Microsoft.EntityFrameworkCore;
using ChurchManager.Domain.Shared;

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

        public async Task<StatisticsViewModel> DashboardChurchConnectionStatusBreakdown(int? churchGroupId = null, int? churchId = null, CancellationToken cancellationToken = default)
        {
            var cacheKey = CacheKeyHelper.CacheKey("DashboardChurchConnectionStatusBreakdown_".ToLower() + (churchGroupId ??= 0) + (churchId ??= 0));
            
            return await _cache.GetOrSetAsync<StatisticsViewModel>(cacheKey, async () =>
            {
                var query = Queryable(false).AsNoTracking();
                
                if (churchGroupId.HasValue)
                {
                    query.Include(x => x.Church).ThenInclude(x => x.ChurchGroup);
                    query = query.Where(x => x.Church.ChurchGroupId == churchGroupId.Value);
                }
            
                if (churchId.HasValue && churchId.Value > 0)
                {
                    query = query.Where(x => x.ChurchId == churchId.Value);
                }
            
                /*var connectionStatus = await query.GroupBy(p => p.ConnectionStatus)
                    .Select(g => new { name = g.Key.Value, count = g.Count() })
                    .ToListAsync(cancellationToken);*/
                
                // Assuming 'query' is your IQueryable or IEnumerable source
                var connectionStatus = await query
                    .GroupBy(p => p.ConnectionStatus)
                    .Select(g => new CountItemViewModel { Name = g.Key.Value, Count = g.Count() })
                    .ToListAsync(cancellationToken);
            
                var gender = await query.GroupBy(p => p.Gender)
                    .Select(g => new CountItemViewModel { Name = g.Key.Value, Count = g.Count() })
                    .ToListAsync(cancellationToken);
            
                var age = await query.GroupBy(p => p.AgeClassification)
                    .Select(g => new  CountItemViewModel { Name = g.Key.Value, Count = g.Count() })
                    .ToListAsync(cancellationToken);
            
                connectionStatus = connectionStatus.OrderBy(x => x.Name).ToList();
                gender = gender.OrderBy(x => x.Name).ToList();
                age = age.OrderBy(x => x.Name).ToList();
                
                var statistics = new StatisticsViewModel();
                statistics.Data.Add("connectionStatus", connectionStatus);
                statistics.Data.Add("gender", gender);
                statistics.Data.Add("age", age);
            
                return statistics;
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

        public Task<PersonViewModelBasic?> BasicPersonViewModelAsync(int personId, CancellationToken cancellationToken = default)
        {
            return Queryable()
                .AsNoTracking()
                .Where(x => x.Id == personId)
                .Select(x => x.ToBasicPersonViewModel())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IList<PersonViewModelBasic?>> BasicPersonsViewModelAsync(IList<int> personIds, CancellationToken cancellationToken = default)
        {
            return await Queryable()
                .AsNoTracking()
                .Where(x => personIds.Contains(x.Id))
                .Select(x => x.ToBasicPersonViewModel())
                .ToListAsync(cancellationToken);
            
            /*{
                   PersonId = x.Id,
                   Title = x.FullName.Title,
                   FirstName = x.FullName.FirstName,
                   LastName = x.FullName.LastName,
                   Gender = x.Gender.ToString(),
                   AgeClassification = x.AgeClassification,
                   PhotoUrl = x.PhotoUrl,
                   Age = x.BirthDate != null ? x.BirthDate.Age : null,
                   Email =x.Email != null ? x.Email.Address : null
               }*/
        }

        public Task<string> FamilyCode(int personId, CancellationToken cancellationToken = default)
        {
            return Queryable()
                .AsNoTracking()
                    .Include(x => x.Family)
                .Where(x=> x.Id == personId)
                .Select(x => x.Family.Code)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Person?> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
        {
            var query = Queryable()
                .AsNoTracking()
                .Include(x => x.PhoneNumbers)
                .Include(x => x.Family)
                .FirstOrDefaultAsync(x => x.PhoneNumbers!.Any(p => p.Number == phoneNumber), cancellationToken);
            
            return await query;
        }
        
        public async Task<PersonViewModelBasic?> FindBasicPersonByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
        {
            var query = Queryable()
                .AsNoTracking()
                .Include(x => x.PhoneNumbers)
                .Include(x => x.Family)
                .Where(x => x.PhoneNumbers!.Any(p => p.Number == phoneNumber))
                .Select(x => x.ToBasicPersonViewModel())
                .FirstOrDefaultAsync(cancellationToken);
            
            return await query;
        }

        public async Task<Dictionary<string, Person?>> FindPhoneNumberForPeople(IList<string> phoneNumbers, CancellationToken ct = default)
        {
            var uniquePhoneNumbers = phoneNumbers.Distinct().ToList();
            
            var people = await Queryable()
                .AsNoTracking()
                .Include(x => x.PhoneNumbers)
                .Include(x => x.Family)
                .Where(x => x.PhoneNumbers!.Any(p => uniquePhoneNumbers.Contains(p.Number!)))
                .ToListAsync(ct);
                
            // Create a dictionary mapping each phone number to its corresponding person
            var phoneNumberPersonMap = new Dictionary<string, Person?>(uniquePhoneNumbers.Count);
            
            // Initialize all requested phone numbers with null (in case some aren't found)
            foreach (var phoneNumber in uniquePhoneNumbers)
            {
                phoneNumberPersonMap[phoneNumber] = null;
            }
            
            // For each person found, map their matching phone numbers to them
            foreach (var person in people)
            {
                var matchingPhoneNumbers = person.PhoneNumbers!
                    .Where(p => phoneNumbers.Any() && phoneNumbers.Contains(p.Number!))
                    .Select(p => p.Number!);
                    
                foreach (var number in matchingPhoneNumbers)
                {
                    phoneNumberPersonMap[number] = person;
                }
            }
            
            return phoneNumberPersonMap;
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