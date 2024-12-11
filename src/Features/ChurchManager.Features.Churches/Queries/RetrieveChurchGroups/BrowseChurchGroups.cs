using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Churches.Specifications;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Churches.Queries.RetrieveChurchGroups;

public record BrowseChurchGroups(string SearchTerm) : IRequest<ApiResponse>;

public class BrowseChurchGroupsQueryHandler : IRequestHandler<BrowseChurchGroups, ApiResponse>
{
    private readonly IGenericDbRepository<ChurchGroup> _dbRepository;

    public BrowseChurchGroupsQueryHandler(IGenericDbRepository<ChurchGroup> dbRepository)
    {
        _dbRepository = dbRepository;
    }
    public async Task<ApiResponse> Handle(BrowseChurchGroups query, CancellationToken ct)
    {
        var spec = new ChurchGroupsQuerySpecification(query.SearchTerm);

        var vm = await _dbRepository.ListAsync<ChurchGroupViewModel>(spec, ct);

        return new ApiResponse(vm);
    }
}