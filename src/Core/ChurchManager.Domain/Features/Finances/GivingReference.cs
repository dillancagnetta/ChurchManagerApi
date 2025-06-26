namespace ChurchManager.Domain.Features.Finances;

public record GivingReference
{
    public int? FamilyId { get; set; }
    public int? PersonId { get; set; }
    public int? ChurchId { get; set; }
    public GivingType GivingType { get; set; } = GivingType.Unknown;
    public BenefactorType BenefactorType { get; set; } = BenefactorType.Unknown;
    public bool IsParsed { get; set; }
}
