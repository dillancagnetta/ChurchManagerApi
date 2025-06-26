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
    Task<(BankStatementImport Import, IList<ImportedTransaction> UnProcessedTransactoion)> ResolveAsync(BankStatementImport import);
    Task<GivingReference> TryResolveChurchAsync(string reference, GivingReference resolvedReference);
    Task<GivingReference> TryResolvePersonAsync(string reference, GivingReference resolvedReference, Dictionary<string, Person?> map);
    
    IList<string> ParsePhoneNumbers(IList<string> references);
}