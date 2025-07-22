using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Finances.Banking;
using ChurchManager.Domain.Features.Finances.Services;
using Codeboss.Results;
using OfxSharp;
using Transaction = ChurchManager.Domain.Features.Finances.Banking.Transaction;

namespace ChurchManager.Infrastructure.Shared.BankImport;

public class OfxBankStatementImporter : IBankStatementImporter
{
    public Task<OperationResult<BankStatementImport>> ImportAsync(Stream fileStream, string fileName, CancellationToken ct = default)
    {
        try
        {
            // Reset to beginning of the stream to be sure
            fileStream.Position = 0;
            var ofxDocument  = OfxDocumentReader.ReadFile(fileStream)!;
        
            var statement = ofxDocument.Statements.FirstOrDefault();
            if (statement == null) return Task.FromResult(OperationResult<BankStatementImport>.Fail("No bank statement found"));

            List<OfxTransactionType> incomingPayments =
                [OfxTransactionType.DEP, OfxTransactionType.CREDIT, OfxTransactionType.PAYMENT, OfxTransactionType.DIRECTDEP, OfxTransactionType.ATM];
        
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
        
            return Task.FromResult(OperationResult<BankStatementImport>.Success(import));
        }
        catch (Exception e)
        {
            return Task.FromResult(OperationResult<BankStatementImport>.Fail(e.Message));
        }
    }

    private Transaction Map(OfxSharp.Transaction ofxTransaction, Currency currency)
    {
        return new Transaction
        {
            TransactionAmount = new Money(currency, ofxTransaction.Amount),
            TransactionDate = ofxTransaction.Date!.Value.DateTime,
            OriginalReference = ofxTransaction.Name,
            BankTransactionId = ofxTransaction.TransactionId, //  <FITID> Unique transaction ID for this statement only (not universal)
            TransactionType = ofxTransaction.TransType.ToString(), //  <TRNTYPE> The type of transaction
            Memo = ofxTransaction.Memo 
        };
    }
}