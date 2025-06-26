using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Finances.Banking;
using ChurchManager.Domain.Features.Finances.Services;
using OfxSharp;

namespace ChurchManager.Infrastructure.Shared.BankImport;

public class OfxBankStatementImporter : IBankStatementImporter
{
    public Task<BankStatementImport> ImportAsync(Stream fileStream, string fileName, CancellationToken ct = default)
    {
        var ofxDocument  = OfxDocumentReader.ReadFile(fileStream)!;
        
        var statement = ofxDocument.Statements.FirstOrDefault() ?? throw new InvalidOperationException("No statements found in OFX file");

        List<OfxTransactionType> incomingPayments =
            [OfxTransactionType.DEP, OfxTransactionType.CREDIT, OfxTransactionType.PAYMENT, OfxTransactionType.DIRECTDEP];
        
        var transactions = statement.Transactions.Where(t => incomingPayments.Contains(t.TransType)).ToList();

        var currency = new Currency(statement.DefaultCurrency);
        
        var import = new BankStatementImport
        {
            FileName = fileName,
            BankAccount = statement.AccountFrom.AccountId,
            ImportDate = DateTime.UtcNow,
            Currency = statement.DefaultCurrency ?? Currency.ZAR.Value,
            TransactionCount = transactions.Count,
            StatementStartDate = statement.TransactionsStart.DateTime,
            StatementEndDate = statement.TransactionsEnd.DateTime,
            Transactions = transactions.Select(t => Map(t, currency)).ToList()
        };
        
        return Task.FromResult(import);
    }

    private ImportedTransaction Map(Transaction ofxTransaction, Currency currency)
    {
        return new ImportedTransaction
        {
            Amount = new Money(currency, ofxTransaction.Amount),
            TransactionDate = ofxTransaction.Date!.Value.DateTime,
            OriginalReference = ofxTransaction.Name,
            BankTransactionId = ofxTransaction.TransactionId, //  <FITID> Unique transaction ID for this statement only (not universal)
            TransactionType = ofxTransaction.TransType.ToString(), //  <TRNTYPE> The type of transaction
            Memo = ofxTransaction.Memo 
        };
    }
}