using ChurchManager.Domain.Features.Finances.Banking;

namespace ChurchManager.Domain.Features.Finances.Services;

public interface IBankStatementProcessor
{
    Task<BankStatementProcessResult> ProcessAsync(BankStatementImport import, CancellationToken ct = default);
}