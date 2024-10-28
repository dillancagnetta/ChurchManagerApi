using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.Groups.Specifications;
using ChurchManager.Domain.Parameters;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Groups.Queries.GroupsAutocomplete;
public record GroupsAutocompleteQuery : SearchTermQueryParameter, IRequest<ApiResponse>;

public class GroupsAutocompleteResults : IRequestHandler<GroupsAutocompleteQuery, ApiResponse>
{
    private readonly IGroupDbRepository _dbRepository;

    public GroupsAutocompleteResults(IGroupDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }
    
    public async Task<ApiResponse> Handle(GroupsAutocompleteQuery query, CancellationToken ct)
    {
        var spec = new GroupsAutocompleteSpecification(query.SearchTerm);
        var results = await _dbRepository.ListAsync(spec, ct);

        return new ApiResponse(results);
    }
}