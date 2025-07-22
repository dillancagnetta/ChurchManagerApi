using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Finances;

public class PaymentTransaction: Entity<int>, IAggregateRoot<int>
{
    /// <summary>
    ///     Gets or sets payment method system name e.g. Payments.PayFast
    /// </summary>
    [MaxLength(100)] public required string PaymentMethodSystemName { get; set; }
    
    [Required, MaxLength(100)] public string? PaymentReference { get; set; }
    
    [Required, MaxLength(255)] public string? Description { get; set; }
    
    /// <summary>
    /// Gets the date when the payment was initiated.
    /// </summary>
    public DateTime InitiatedDate { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Gets or sets the monetary amount of the contribution.
    /// </summary>
    public Money? PaidAmount  { get; set; }
    
    public Money? RefundedAmount  { get; set; }
    
    public Money? Fee { get; set; }
        
    public Money? NetAmount { get; set; }

    /// <summary>
    ///     Gets or sets transaction status
    /// </summary>
    [Required, MaxLength(100)]
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    
    /// <summary>
    /// Gets or sets the method used to make the payment (e.g., cash, check, credit card).
    /// </summary>
    [Required, MaxLength(100)] public PaymentMethod PaymentMethod { get; set; }

    [MaxLength(255)] public string? PaymentId { get; set; } = Guid.NewGuid().ToString("D");
    
    /// <summary>
    /// Gets an external reference identifier for the contribution, such as a transaction ID from a payment processor.
    /// </summary>
    [MaxLength(255)] public string? ExternalReferenceId { get; set; }
    
    [MaxLength(255)] public string? Error { get; set; }
    
    public DateTime? CompletedDate { get; set; }
    
    /// <summary>
    /// Indicates whether this payment has been converted to a Giving record
    /// </summary>
    public bool ConvertedToGiving { get; set; } = false;
    
    public bool IsTest { get; set; } = false;
    
    /// <summary>
    /// Reference to the Giving record created from this payment (if any)
    /// </summary>
    public int? GivingId { get; set; }
    
    /// <summary>
    /// The Church the giving is originated from.
    /// </summary>
    public int? ChurchId { get; set; }
    
    public int? PersonId { get; set; }
    [MaxLength(100)] public string? FirstName { get; set; }
    [MaxLength(100)] public string? LastName { get; set; }

    
    public virtual Giving? Giving { get; set; }
    public virtual Church? Church { get; set; }
}