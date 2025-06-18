//using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Features.People.Queries;
using Convey.CQRS.Queries;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Application.Abstractions.Services;

public interface IPersonService
{
    Task<PagedResult<PersonViewModel>> BrowseAsync(PeopleAdvancedSearchQuery query, CancellationToken ct = default);
    Task<IReadOnlyList<PeopleAutocompleteViewModel>> PeopleAutocompleteAsync(string searchTerm, CancellationToken ct = default);
    Task<IReadOnlyList<PersonViewModel>> FilterPeopleAsync(IList<int> personIds, CancellationToken ct = default);
}