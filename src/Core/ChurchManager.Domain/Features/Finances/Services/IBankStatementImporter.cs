using ChurchManager.Domain.Features.Finances.Banking;

namespace ChurchManager.Domain.Features.Finances.Services;

public interface IBankStatementImporter
{
    Task<BankStatementImport> ImportAsync(
        Stream fileStream, 
        string fileName, 
        CancellationToken ct = default);
}