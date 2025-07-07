using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Common;
using ChurchManager.Persistence.Shared;
using CodeBoss.Extensions;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Finances.Banking;

public class BankStatementImport: AuditableEntity<int>, IAggregateRoot<int>
{
    [Required, MaxLength(255)] public required string FileName { get; set; }
    [Required, MaxLength(100)] public required string BankAccount { get; set; }
    [Required] public DateTime ImportDate { get; set; }
    [Required, MaxLength(3)] public required Currency Currency { get; set; } // "ZAR"
    public int TransactionCount { get; set; }
    public int ErrorCount { get; set; }
    
    [MaxLength(500)] public string? Errors { get; set; }
    public bool IsCompleted { get; set; }
    
    public DateTime StatementStartDate { get; set; }  // From OFX
    public DateTime StatementEndDate { get; set; }    // From OFX

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public virtual ICollection<Giving> Givings { get; set; } = new List<Giving>();
    //public virtual ICollection<Transaction> UnProcessedTransactions { get; set; } = new List<Transaction>();
    
    public IList<string> OriginalTransactionReferences()
    {
        return Transactions.Select(t => t.OriginalReference).ToList();
    }
    
    public int ProcessedCount() => Transactions.Count(t => t.IsProcessed is true);
    public int UnProcessedCount() => Transactions.Count(t =>t.IsProcessed is null or false);

    public void AddError(string? error)
    {
        ErrorCount++;
        if (Errors.IsNullOrEmpty())
        {
            Errors = error;
            return;
        }
 
        Errors += $", {error}";
    }

    public void AddGiving(Giving giving)
    {
        Givings.Add(giving);
    }
}

public class Transaction : AuditableEntity<int>
{
    public int ImportId { get; set; }
    [Required, MaxLength(50)] public required string OriginalReference { get; set; }
    [Required] public required Money TransactionAmount { get; set; }
    [Required] public DateTime TransactionDate { get; set; }
    [Required, MaxLength(150)] public required string BankTransactionId { get; set; }
    [Required, MaxLength(150)] public required string TransactionType { get; set; }
    
    // Resolution status
    public bool IsMatched { get; set; }
    public bool? IsProcessed { get; set; }
    
    public bool? IsResolved { get; set; }
    [MaxLength(500)] public string? ResolutionNotes { get; set; }
    
    [MaxLength(250)] public string? Memo { get; set; }
    [MaxLength(250)] public string? Error { get; set; }
    
    #region Navigation
    public virtual BankStatementImport? Import { get; set; }
    #endregion

    public void SetAsUnProcessed(string? error)
    {
        IsProcessed = false;
        Error = error;
    }

    public void SetAsProcessed()
    {
        IsProcessed = true;
    }
}