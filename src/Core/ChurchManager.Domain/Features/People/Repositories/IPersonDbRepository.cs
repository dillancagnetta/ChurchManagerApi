using ChurchManager.Domain.Features.People.Queries;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Codeboss.Results;

namespace ChurchManager.Domain.Features.People.Repositories
{
    public interface IPersonDbRepository : IGenericDbRepository<Person>
    {
        IQueryable<Person> FindPersons(PersonMatchQuery searchParameters, bool includeDeceased = false, params string[] includes);
        IQueryable<Person> Queryable(bool includeDeceased);
        IQueryable<Person> Queryable(PersonQueryOptions personQueryOptions);
        Task<dynamic> DashboardChurchConnectionStatusBreakdown(int? churchId = null, CancellationToken cancellationToken = default);
        Task<OperationResult<Guid?>> UserLoginIdForPersonAsync(int  personId, CancellationToken cancellationToken = default);
    }
}
