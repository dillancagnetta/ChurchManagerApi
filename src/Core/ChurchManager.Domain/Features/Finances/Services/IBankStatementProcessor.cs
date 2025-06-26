using ChurchManager.Domain.Features.Finances.Banking;

namespace ChurchManager.Domain.Features.Finances.Services;

public interface IBankStatementProcessor
{
    Task<(BankStatementImport Import, IList<ImportedTransaction> UnProcessedTransactoion)> ProcessAsync(BankStatementImport import, CancellationToken ct = default);
}