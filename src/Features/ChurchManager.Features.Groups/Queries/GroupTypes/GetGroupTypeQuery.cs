using ChurchManager.Domain.Features.Groups;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ChurchManager.Application.ViewModels;

namespace ChurchManager.Features.Groups.Queries.GroupTypes;

public record GetGroupTypeQuery(int GroupTypeId) : IRequest<ApiResponse>;

public class GetGroupTypeHandler : IRequestHandler<GetGroupTypeQuery, ApiResponse>
{
    private readonly IGenericDbRepository<GroupType> _dbRepository;
    private readonly IMapper _mapper;

    public GetGroupTypeHandler(IGenericDbRepository<GroupType> dbRepository, IMapper mapper)
    {
        _dbRepository = dbRepository;
        _mapper = mapper;
    }
    
    public async Task<ApiResponse> Handle(GetGroupTypeQuery query, CancellationToken ct)
    {
        // Single
        var groupType = await _dbRepository.Queryable()
            .FirstOrDefaultAsync(g => g.Id == query.GroupTypeId, ct);
        var mapped = _mapper.Map<GroupTypeViewModel>(groupType);

        return new ApiResponse(mapped);
    }
}