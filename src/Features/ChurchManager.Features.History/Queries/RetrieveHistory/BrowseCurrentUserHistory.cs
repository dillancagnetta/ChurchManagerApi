using AutoMapper;
using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Features.History;
using ChurchManager.Domain.Features.History.Specifications;
using ChurchManager.Domain.Parameters;
using ChurchManager.SharedKernel.Wrappers;
using Convey.CQRS.Queries;
using MediatR;

namespace ChurchManager.Features.History.Queries.RetrieveHistory;

public record BrowseCurrentUserHistory(string EntityType, int PersonId) : QueryParameter,  IRequest<PagedResponse<HistoryViewModel>>;

public class BrowseCurrentUserHistoryHandler : IRequestHandler<BrowseCurrentUserHistory, PagedResponse<HistoryViewModel>>
{
    private readonly IHistoryDbRepository _dbRepository;
    private readonly IMapper _mapper;

    public BrowseCurrentUserHistoryHandler(IHistoryDbRepository dbRepository, IMapper mapper)
    {
        _dbRepository = dbRepository;
        _mapper = mapper;
    }
    public async Task<PagedResponse<HistoryViewModel>> Handle(BrowseCurrentUserHistory query, CancellationToken ct)
    {
        var spec = new HistoryQuerySpecification(query, query.EntityType, query.PersonId);
        
        var pagedResult = await _dbRepository.BrowseAsync(query, spec, ct);
        
        var vm = _mapper.Map<PagedResult<HistoryViewModel>>(pagedResult);
        
        return new PagedResponse<HistoryViewModel>(vm);
    }
}