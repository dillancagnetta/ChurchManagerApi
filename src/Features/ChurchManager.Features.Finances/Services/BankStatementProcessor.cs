using ChurchManager.Domain.Features.Finances.Banking;
using ChurchManager.Domain.Features.Finances.Services;

namespace ChurchManager.Features.Finances.Services;

public class BankStatementProcessor(IGivingReferenceResolver referenceResolver) : IBankStatementProcessor
{
    public async Task<(BankStatementImport Import, IList<ImportedTransaction> UnProcessedTransactoion)> ProcessAsync(BankStatementImport import, CancellationToken ct = default)
    {
        var importAdjusted = await referenceResolver.ResolveAsync(import);
        
        return importAdjusted;
    }
}