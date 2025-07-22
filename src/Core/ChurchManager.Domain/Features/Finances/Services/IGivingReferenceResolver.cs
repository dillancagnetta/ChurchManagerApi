using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Finances.Banking;
using ChurchManager.Domain.Features.People;

namespace ChurchManager.Domain.Features.Finances.Services;

public interface IGivingReferenceResolver
{
    /// <summary>
    ///Example formats:
    ///  Person: CHU-082xxxxxxx-T
    ///  Family: CHU-082xxxxxxx-P-HS-F
    /// </summary>
    Task<BankStatementProcessResult> ResolveAsync(BankStatementImport import, CancellationToken ct =  default);
    
    /// <summary>
    /// Try resolve church using the short code in the reference.
    /// </summary>
    Task<GivingReference> TryResolveChurchAsync(string reference, GivingReference resolvedReference, CancellationToken ct =  default);
    
    /// <summary>
    /// Try resolve person using the phone number in the reference.
    /// </summary>
    Task<GivingReference> TryResolvePersonAsync(string reference, GivingReference resolvedReference, Dictionary<string, Person?> map, CancellationToken ct =  default);
    
    /// <summary>
    /// Try resolve person using the phone number in the reference.
    /// </summary>
    Task<GivingReference> TryResolvePersonAsync(string reference, CancellationToken ct =  default);
    
    Task<Fund> ResolveFundAsync(GivingType fundCode, string partnershipFund, CancellationToken ct = default);
    
    /// <summary>
    /// Extract and Parses (cleans) phone numbers from the given list of references.
    /// </summary>
    IList<string> ParsePhoneNumbers(IList<string> references);

    Task<(Giving? giving, bool IsMatched, string? ErrorMessage)> ResolveToGivingAsync(
        string reference,
        Money money,
        PaymentMethod paymentMethod,
        CancellationToken ct = default);
}