namespace ChurchManager.Domain.Features.Finances;

public record GivingReference
{
    public BeneficiaryInfo? Family { get; set; }
    public BeneficiaryInfo? Person { get; set; }
    public BeneficiaryInfo? Church { get; set; }
    public GivingType GivingType { get; set; } = GivingType.Unknown;
    public BenefactorType BenefactorType { get; set; } = BenefactorType.Unknown;

    public string? OriginalReference { get; set; }
    // if not Parsed we need to place in a general or default Fund
    public bool IsChurchMatched { get; set; }
    public bool IsPersonMatched { get; set; }
}

public record BeneficiaryInfo(int Id, string Name);