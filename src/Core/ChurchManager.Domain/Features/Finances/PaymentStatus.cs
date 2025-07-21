using Codeboss.Types;

namespace ChurchManager.Domain.Features.Finances;

public class PaymentStatus : Enumeration<PaymentStatus, string>
{
    public PaymentStatus() { Value = "Pending"; }
        
    public PaymentStatus(string value) => Value = value;

    public static PaymentStatus Pending = new("Pending");
    public static PaymentStatus Processing = new("Processing");
    public static PaymentStatus Authorized = new("Authorized");
    public static PaymentStatus PartialPaid = new("PartialPaid");
    public static PaymentStatus Completed = new("Completed");
    public static PaymentStatus PartiallyRefunded = new("PartiallyRefunded");
    public static PaymentStatus Refunded = new("Refunded");
    public static PaymentStatus Voided = new("Voided");
    public static PaymentStatus Cancelled = new("Cancelled");
    public static PaymentStatus Failed = new("Failed");
    // Implicit conversion from string
    public static implicit operator PaymentStatus(string value) => new(value);
}