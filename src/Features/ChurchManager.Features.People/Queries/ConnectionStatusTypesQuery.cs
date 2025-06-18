using ChurchManager.Domain.Features.People;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Features.People.Queries;

public record ConnectionStatusTypesQuery : IRequest<ApiResponse>;

public class ConnectionStatusTypesHandler(IGenericDbRepository<ConnectionStatusType> dbRepository) : IRequestHandler<ConnectionStatusTypesQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(ConnectionStatusTypesQuery request, CancellationToken ct)
    {
        var items = await dbRepository.Queryable().AsNoTracking().Select(x => new SelectItemViewModel
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description
        }).ToListAsync(ct);
        
        return new ApiResponse(items);
    }
}