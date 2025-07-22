using Codeboss.Types;

namespace ChurchManager.Domain.Features.Finances;

public class BenefactorType : Enumeration<BenefactorType, string>
{
    public BenefactorType() { Value = "Individual"; }
        
    public BenefactorType(string value) => Value = value;

    public static BenefactorType Individual = new("Individual");
    public static BenefactorType Family = new("Family");
    public static BenefactorType Group = new("Group");
    public static BenefactorType Church = new("Church");
    public static BenefactorType External = new("External");
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
    public static PaymentMethod  DebitCard = new("Debit Card");
    public static PaymentMethod  CreditCard = new("Credit Card");
    public static PaymentMethod  MobilePhone = new("Mobile Phone"); // Apple , Samsung, Google Pay etc.
    public static PaymentMethod  Unknown = new("Unknown");
    // Implicit conversion from string
    public static implicit operator PaymentMethod (string value) => new(value);
    
    public static PaymentMethod FromTransactionType(string? type) =>
        (type ?? string.Empty) switch
        {
            "CREDIT" => EFT,
            "DEPOSIT"  => Cash,
            "PAYMENT"  => EFT,
            "DIRECT DEPOSIT"  => Cash,
            "ATM TRANSFER"  => Cash,
            _    => Unknown
        };
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