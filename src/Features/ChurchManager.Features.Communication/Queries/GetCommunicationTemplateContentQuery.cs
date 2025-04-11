using ChurchManager.Domain.Features.Security.Services;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Common;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Feature = ChurchManager.Domain.Features.Communications;

namespace ChurchManager.Features.Communication.Queries;

public record GetCommunicationTemplateContentQuery(int CommunicationTemplateId) : IRequest<ApiResponse>;

public class etCommunicationTemplateHandler(
    IReadDbRepository<Feature.CommunicationTemplate> dbRepository,
    IPermissionContext permissions,
    IAppCurrentUser currentUser) : IRequestHandler<GetCommunicationTemplateContentQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetCommunicationTemplateContentQuery query, CancellationToken ct)
    {
       var templateContent = await dbRepository.Queryable().AsNoTracking()
           .Where(x => x.Id == query.CommunicationTemplateId)
           .Select(x => x.Content)
           .FirstOrDefaultAsync(ct);
       
       return new ApiResponse{ Data = templateContent };
    }
}