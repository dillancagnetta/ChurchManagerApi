using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Finances;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Infrastructure.Persistence.Extensions;
using ChurchManager.SharedKernel.Wrappers;
using JasperFx.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.Finances.Commands;

public record GeneratePaymentReferenceCommand : IRequest<ApiResponse>
{
    public required ChurchReference ChurchReference { get; set; }
    public int FundId { get; set; }
    [MinLength(10)] public required string PhoneNumber { get; set; }
    [MinLength(6)] public required string BenefactorType { get; set; } // Individual, Family, etc.
}

public class GeneratePaymentReferenceHandler(
    IReadDbRepository<Church> churchDb,
    IReadDbRepository<Fund> fundDb,
    IQueryCache cache) : IRequestHandler<GeneratePaymentReferenceCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(GeneratePaymentReferenceCommand command, CancellationToken ct)
    {
        if (!command.PhoneNumber.IsValidNumber())
        {
            return new ApiResponse("Invalid phone number format.");
        }

        var churchCacheKey =
            $"{CacheKeyHelper.CacheKey<GeneratePaymentReferenceCommand>("churches")}-{command.ChurchReference.ChurchGroupId}";
        var churches = await cache.GetOrSetAsync(churchCacheKey,
            () =>  churchDb.Queryable().AsNoTracking()
                .Where(x => x.ChurchGroupId == command.ChurchReference.ChurchId)
                .Select(x => new { x.Id, x.ShortCode }).ToListAsync(ct), ct: ct);
        
        var funds = await cache.GetOrSetAsync(CacheKeyHelper.CacheKey<GeneratePaymentReferenceCommand>("funds"),
            () =>  fundDb.Queryable().AsNoTracking().Select(x => new { x.Id, x.Code, x.FundType }).ToListAsync(ct), ct: ct);
        
        var churchShortCode = churches.First(x => x.Id == command.ChurchReference.ChurchId).ShortCode;
        var fund = funds.First(x => x.Id == command.FundId);
        var fundsShortCode = fund.Code;
        
        // Start with base reference
        var reference = $"{churchShortCode}-{command.PhoneNumber}";

        // Add Partnership suffix
        if (fund.FundType == FundType.Partnership)
        {
            reference += $"-P-{fundsShortCode}";
        }
        else
        {
            // For non-partnership funds (General, Offerings, etc.)
            reference += $"-{fundsShortCode}";
        }

        // Add family suffix if applicable
        if (command.BenefactorType == BenefactorType.Family.Value)
        {
            reference += "-F";
        }

        return new ApiResponse(reference, null);    
    }
}