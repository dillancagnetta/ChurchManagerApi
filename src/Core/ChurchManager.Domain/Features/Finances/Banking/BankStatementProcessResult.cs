using Codeboss.Results;

namespace ChurchManager.Domain.Features.Finances.Banking;

public class BankStatementProcessResult : OperationResult<BankStatementProcessResult>
{
    public required BankStatementImport Import { get; set; }
    public IList<ImportedTransaction> UnProcessedTransactions { get; set; } = new List<ImportedTransaction>(0);
}