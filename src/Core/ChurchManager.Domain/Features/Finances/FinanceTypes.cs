using Codeboss.Types;

namespace ChurchManager.Domain.Features.Finances;

public class GivingType : Enumeration<GivingType, string>
{
    public GivingType() { Value = "Unknown"; }
        
    public GivingType(string value) => Value = value;

    public static GivingType Tithe = new("Tithe");
    public static GivingType Offering = new("Offering");
    public static GivingType Seed = new("Seed");
    public static GivingType FirstFruit = new("First Fruit");
    public static GivingType Unknown = new("Unknown");
    // Implicit conversion from string
    public static implicit operator GivingType(string value) => new(value);

    public static GivingType FromInitials(string? initials) =>
        (initials ?? string.Empty).ToUpperInvariant() switch
        {
            "FF" => FirstFruit,
            "T"  => Tithe,
            "O"  => Offering,
            "S"  => Seed,
            _    => Unknown
        };
}

public class BenefactorType : Enumeration<BenefactorType, string>
{
    public BenefactorType() { Value = "Individual"; }
        
    public BenefactorType(string value) => Value = value;

    public static BenefactorType Individual = new("Individual");
    public static BenefactorType Family = new("Family");
    public static BenefactorType Group = new("Group");
    public static BenefactorType Church = new("Church");
    public static BenefactorType Unknown = new("Unknown");
    // Implicit conversion from string
    public static implicit operator BenefactorType(string value) => new(value);
}

public class PaymentMethod : Enumeration<PaymentMethod, string>
{
    public PaymentMethod () { Value = "EFT"; }
        
    public PaymentMethod (string value) => Value = value;

    public static PaymentMethod  EFT = new("EFT");
    public static PaymentMethod  Cash = new("Cash");
    public static PaymentMethod  Digital = new("Digital");
    public static PaymentMethod  Unknown = new("Unknown");
    // Implicit conversion from string
    public static implicit operator PaymentMethod (string value) => new(value);
}

// Categorizes WHERE the money goes (the purpose/destination)
public class FundType : Enumeration<FundType, string>
{
    public FundType () { Value = "General"; }
        
    public FundType (string value) => Value = value;

    public static FundType  General = new("General");
    public static FundType  Partnership = new("Partnership");
    public static FundType  ChurchProjects = new("ChurchProjects");
    public static FundType  Unknown = new("Unknown");
    
    // Implicit conversion from string
    public static implicit operator FundType (string value) => new(value);
}