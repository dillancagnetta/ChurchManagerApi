using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Common;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Finances.Banking;

public class BankStatementImport: AuditableEntity<int>, IAggregateRoot<int>
{
    [Required, MaxLength(255)] public required string FileName { get; set; }
    [Required, MaxLength(100)] public required string BankAccount { get; set; }
    [Required] public DateTime ImportDate { get; set; }
    [Required, MaxLength(3)] public required Currency Currency { get; set; } // "ZAR"
    public int TransactionCount { get; set; }
    public int ProcessedCount { get; set; }
    public int UnmatchedCount { get; set; }
    public int ErrorCount { get; set; }
    [MaxLength(500)] public string? Errors { get; set; }
    public bool IsCompleted { get; set; }
    
    public DateTime StatementStartDate { get; set; }  // From OFX
    public DateTime StatementEndDate { get; set; }    // From OFX

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    //public virtual ICollection<Transaction> UnProcessedTransactions { get; set; } = new List<Transaction>();
    
    public IList<string> OriginalTransactionReferences()
    {
        return Transactions.Select(t => t.OriginalReference).ToList();
    }

    /*public void AddProcessedTransaction(Transaction transaction)
    {
        Transactions.Add(transaction);
    }
    
    public void AddUnProcessedTransaction(Transaction transaction)
    {
        UnProcessedTransactions.Add(transaction);
    }*/
}

public class Transaction : AuditableEntity<int>
{
    public int ImportId { get; set; }
    [Required, MaxLength(50)] public required string OriginalReference { get; set; }
    [Required] public required Money Amount { get; set; }
    [Required] public DateTime TransactionDate { get; set; }
    [Required, MaxLength(150)] public required string BankTransactionId { get; set; }
    [Required, MaxLength(150)] public required string TransactionType { get; set; }
    
    // Resolution status
    public bool IsMatched { get; set; }
    public bool? IsProcessed { get; set; }
    public int? GivingId { get; set; }  // null if unmatched
    [MaxLength(50)] public string? ParsedReference { get; set; }
    
    public bool? IsResolved { get; set; }
    [MaxLength(500)] public string? ResolutionNotes { get; set; }
    
    [MaxLength(250)] public string? Memo { get; set; }
    [MaxLength(250)] public string? Error { get; set; }
    
    #region Navigation
    public virtual BankStatementImport? Import { get; set; }
    public virtual Giving? Giving { get; set; }
    #endregion

    public void SetAsUnProcessed()
    {
        IsProcessed = false;
    }

    public void SetAsProcessed()
    {
        IsProcessed = true;
    }
}