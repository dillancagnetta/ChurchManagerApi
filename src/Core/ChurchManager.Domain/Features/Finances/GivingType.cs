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
    public static GivingType Partnership = new("Partnership");
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
            "P"  => Partnership,
            _    => Unknown
        };
}