using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Features.People.Specifications;
using ChurchManager.Domain.Parameters;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.People.Queries.PeopleAutocomplete
{
    public record PeopleAutocompleteQuery : SearchTermQueryParameter, IRequest<ApiResponse>;

    public class PeopleAutocompleteResults : IRequestHandler<PeopleAutocompleteQuery, ApiResponse>
    {
        private readonly IPersonService _service;

        public PeopleAutocompleteResults(IPersonService service)
        {
            _service = service;
        }

        public async Task<ApiResponse> Handle(PeopleAutocompleteQuery query, CancellationToken ct)
        {
            /*var spec = new PeopleAutocompleteSpecification(query.SearchTerm);
            var results = await _dbRepository.ListAsync(spec, ct);
            */
            
            var results = await _service.PeopleAutocompleteAsync(query.SearchTerm, ct);

            return new ApiResponse(results);
        }
    }
}