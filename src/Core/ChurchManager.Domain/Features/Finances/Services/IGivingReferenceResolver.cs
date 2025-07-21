using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Finances.Banking;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Finances.Services;

public interface IGivingReferenceResolver
{
    /// <summary>
    ///Example formats:
    ///  Person: CHU-082xxxxxxx-T
    ///  Family: CHU-082xxxxxxx-P-HS-F
    /// </summary>
    Task<BankStatementProcessResult> ResolveAsync(BankStatementImport import);
    Task<GivingReference> TryResolveChurchAsync(string reference, GivingReference resolvedReference);
    Task<GivingReference> TryResolvePersonAsync(string reference, GivingReference resolvedReference, Dictionary<string, Person?> map);
    Task<PersonViewModelBasic?> TryResolvePersonAsync(string reference, CancellationToken ct);
    
    IList<string> ParsePhoneNumbers(IList<string> references);

    Task<(Giving? giving, bool IsMatched, string? ErrorMessage)> ResolveToGivingAsync(
        string reference,
        Money money,
        PaymentMethod paymentMethod,
        CancellationToken ct = default);
}