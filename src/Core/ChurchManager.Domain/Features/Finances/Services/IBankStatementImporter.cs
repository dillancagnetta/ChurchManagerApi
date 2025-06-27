using ChurchManager.Domain.Features.Finances.Banking;
using Codeboss.Results;

namespace ChurchManager.Domain.Features.Finances.Services;

public interface IBankStatementImporter
{
    Task<OperationResult<BankStatementImport>> ImportAsync(
        Stream fileStream, 
        string fileName, 
        CancellationToken ct = default);
}