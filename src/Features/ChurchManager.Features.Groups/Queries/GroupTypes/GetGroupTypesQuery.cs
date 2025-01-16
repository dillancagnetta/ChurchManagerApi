using AutoMapper;
using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.Groups.Queries.GroupTypes;

public record GetGroupTypesQuery : IRequest<ApiResponse>
{
    public string? Name { get; set; }  = null;
    public string? Description { get; set; } = null;
    public string? GroupTerm { get; set; }  = null;
    public bool? IsSystem { get; set; } = null;
    public bool? TakesAttendance { get; set; } = null;
}

public class GetGroupTypesHandler : IRequestHandler<GetGroupTypesQuery, ApiResponse>
{
    private readonly IGenericDbRepository<GroupType> _dbRepository;
    private readonly IMapper _mapper;

    public GetGroupTypesHandler(IGenericDbRepository<GroupType> dbRepository, IMapper mapper)
    {
        _dbRepository = dbRepository;
        _mapper = mapper;
    }
    
    public async Task<ApiResponse> Handle(GetGroupTypesQuery getQuery, CancellationToken ct)
    {
        var query = _dbRepository.Queryable().AsNoTracking();

        if (!string.IsNullOrEmpty(getQuery.Name))
        {
            query = query.Where(g => g.Name == getQuery.Name);
        }
        
        if (!string.IsNullOrEmpty(getQuery.Description))
        {
            query = query.Where(g => g.Description == getQuery.Description);
        }
        
        if (!string.IsNullOrEmpty(getQuery.GroupTerm))
        {
            query = query.Where(g => g.GroupTerm == getQuery.GroupTerm);
        }
        
        if (getQuery.IsSystem is not null)
        {
            query = query.Where(g => g.IsSystem == getQuery.IsSystem.Value);
        }
        
        if (getQuery.TakesAttendance is not null)
        {
            query = query.Where(g => g.TakesAttendance == getQuery.TakesAttendance.Value);
        }

        var groupTypes = await query.ToListAsync(ct);
        
        var mapped = _mapper.Map<List<GroupTypeViewModel>>(groupTypes);

        return new ApiResponse(mapped.OrderBy(g => g.Name));
    }
}