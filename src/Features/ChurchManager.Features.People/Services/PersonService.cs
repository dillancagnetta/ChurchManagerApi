using AutoMapper;
using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Queries;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Features.People.Specifications;
using ChurchManager.Domain.Features.Permissions;
using ChurchManager.Domain.Features.Permissions.Services;
using ChurchManager.Domain.Shared;
using ChurchManager.SharedKernel.Common;
using Convey.CQRS.Queries;

namespace ChurchManager.Features.People.Services
{
    public class PersonService(
        IPersonDbRepository dbRepository,
        IMapper mapper, 
        IPermissionContext permissions,
        ICognitoCurrentUser currentUser) : IPersonService
    {

        public async Task<PagedResult<PersonViewModel>> BrowseAsync(PeopleAdvancedSearchQuery query,
            CancellationToken ct = default)
        {
            var spec = new BrowsePeopleSpecification(query);

            var pagedResult = await dbRepository.BrowseAsync(query, spec, ct);

            var vm = mapper.Map<PagedResult<PersonViewModel>>(pagedResult);

            return vm;
        }

        public async Task<IReadOnlyList<PeopleAutocompleteViewModel>> PeopleAutocompleteAsync(string searchTerm, CancellationToken ct = default)
        {
            var allowedIds = await permissions.GetAllowedIdsAsync<Person>(
                Guid.Parse(currentUser.Id), PermissionAction.View,   ct);
            
            var spec = new PeopleAutocompleteSpecification(searchTerm, allowedIds);
            
            var vm = await dbRepository.ListAsync(spec, ct);
            
            return vm;
        }
    }
}