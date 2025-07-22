using Codeboss.Results;

namespace ChurchManager.Domain.Features.Finances.Banking;

public class BankStatementProcessResult(BankStatementImport import) : OperationResult<BankStatementImport>(import)
{
}

public record BankStatementProcessState
{
    public required BankStatementImport Import { get; set; }
    public IList<Transaction> UnProcessedTransactions { get; set; } = new List<Transaction>(0);
}