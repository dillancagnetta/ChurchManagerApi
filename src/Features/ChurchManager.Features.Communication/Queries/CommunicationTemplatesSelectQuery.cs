using System.Linq.Dynamic.Core;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using CodeBoss.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Feature = ChurchManager.Domain.Features.Communications;

using NpgsqlTypes;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace ChurchManager.Features.Communication.Queries;

public record CommunicationTemplatesSelectQuery: IRequest<ApiResponse>
{
    public bool IncludeBaseTemplates { get; set; } = false;
    public List<string> CommunicationTypes { get; set; } = [];
}

public class CommunicationTempatesSelectHandler(
    IReadDbRepository<Feature.CommunicationTemplate> dbRepository
    ) : IRequestHandler<CommunicationTemplatesSelectQuery, ApiResponse>
{
    public async Task<ApiResponse> Handle(CommunicationTemplatesSelectQuery query, CancellationToken ct)
    {
        var templates = await dbRepository.Queryable().AsNoTracking()
            .Where(x => query.IncludeBaseTemplates || !x.IsBaseTemplate)
            .Select(x => new
            {
                Id = x.Id,
                Name = x.Name.SplitCase(),
                Description = x.Description,
                SupportedTypes = x.SupportedTypes
            }).ToListAsync(ct);

        if (!query.CommunicationTypes.IsNullOrEmpty())
        {
            templates = templates.Where(e => query.CommunicationTypes
                .Any(requestedType => e.SupportedTypes.Select(x=> x.ToString())
                    .Select(supportedType => supportedType)
                    .Contains(requestedType)))
                .ToList();
        }

        var vm = templates.Select(x => new SelectItemViewModel
        {
            Id = x.Id,
            Name = x.Name.SplitCase(),
            Description = x.Description,
        }).ToList();
        
        return new ApiResponse(vm);
    }
}