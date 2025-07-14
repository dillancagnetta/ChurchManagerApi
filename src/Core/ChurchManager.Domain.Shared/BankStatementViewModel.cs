namespace ChurchManager.Domain.Shared;

public record BankStatementViewModel
{
    public int? Id { get; set; }
    public required string FileName { get; set; }
    public required string BankAccount { get; set; }
    public DateTime ImportDate { get; set; }
    public required string Currency { get; set; } // "ZAR"
    public int TransactionCount { get; set; }
    public int ErrorCount { get; set; }
    public int ProcessedCount { get; set; }
    public int UnProcessedCount { get; set; }
    public bool IsCompleted { get; set; }

    public DateTime StatementStartDate { get; set; }  // From OFX
    public DateTime StatementEndDate { get; set; }    // From OFX

    public IEnumerable<TransactionViewModel?> Transactions { get; set; } = new List<TransactionViewModel>(0);
    public IEnumerable<GivingViewModel?> Givings { get; set; } = new List<GivingViewModel>(0);
};

public record TransactionViewModel
{
    public int? Id { get; set; }
    public int ImportId { get; set; }
    public required string OriginalReference { get; set; }
    public required MoneyViewModel Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public required string BankTransactionId { get; set; }
    public required string TransactionType { get; set; }
    public bool IsMatched { get; set; }
    public bool? IsProcessed { get; set; }
    public bool? IsResolved { get; set; }
    public string? Memo { get; set; }
    public string? Error { get; set; }
    // null if unmatched
    public int? GivingId { get; set; } 
    public GivingViewModel? Giving { get; set; }
}

public record GivingViewModel
{
    public int? Id { get; set; }
    
    public BenefactorViewModel? Benefactor { get; set; }
    public DateTime Date { get; set; }
    public required MoneyViewModel Amount  { get; set; }
    public required string PaymentMethod { get; set; }
    public required string GivingType { get; set; }
    public string? Notes { get; set; }
    public bool ReceiptSent { get; set; }
    public string? ParsedReference { get; set; }
    public string? BankTransactionId { get; set; }
    // Fund
    public int FundId { get; set; }
    public FundViewModel? Fund { get; set; }
}

public record FundViewModel
{
    public int? Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Code { get; set; }
    public required string FundType { get; set; }
    
    // Hierarchy
    public int? ParentFundId { get; set; }
    public IList<FundViewModel> Funds { get; set; } = [];
}

public record BenefactorViewModel
{
    public int? Id { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    
    // Reference IDs to original entities
    public int? PersonId { get;  set; }
    public int? FamilyId { get;  set; }
    public int? GroupId { get; set; }
    public int? ChurchId { get;  set; }
}