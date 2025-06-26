using ChurchManager.Domain.Features.Finances.Banking;

namespace ChurchManager.Domain.Features.Finances.Services;

public interface IBankStatementProcessor
{
    Task<BankStatementImport> ProcessAsync(BankStatementImport import, CancellationToken ct = default);
}