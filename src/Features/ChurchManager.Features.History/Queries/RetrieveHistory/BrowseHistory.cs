using AutoMapper;
using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Features.History;
using ChurchManager.Domain.Features.History.Specifications;
using ChurchManager.Domain.Parameters;
using ChurchManager.SharedKernel.Wrappers;
using Convey.CQRS.Queries;
using MediatR;

namespace ChurchManager.Features.History.Queries.RetrieveHistory;

public record BrowseHistory(string EntityType, int EntityId) : QueryParameter,  IRequest<PagedResponse<HistoryViewModel>>;

public class HistoryQueryHandler : IRequestHandler<BrowseHistory, PagedResponse<HistoryViewModel>>
{
    private readonly IHistoryDbRepository _dbRepository;
    private readonly IMapper _mapper;

    public HistoryQueryHandler(IHistoryDbRepository dbRepository, IMapper mapper)
    {
        _dbRepository = dbRepository;
        _mapper = mapper;
    }
    public async Task<PagedResponse<HistoryViewModel>> Handle(BrowseHistory query, CancellationToken ct)
    {
        var spec = new HistoryQuerySpecification(query, query.EntityType, query.EntityId);
        
        var pagedResult = await _dbRepository.BrowseAsync(query, spec, ct);
        
        var vm = _mapper.Map<PagedResult<HistoryViewModel>>(pagedResult);
        
        return new PagedResponse<HistoryViewModel>(vm);
    }
}