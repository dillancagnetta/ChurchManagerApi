using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Common;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Finances;

public class PaymentTransaction: Entity<int>, IAggregateRoot<int>
{
    /// <summary>
    ///     Gets or sets payment method system name
    /// </summary>
    [MaxLength(100)] public required string PaymentMethodSystemName { get; set; }
    
    /// <summary>
    /// Gets or sets the monetary amount of the contribution.
    /// </summary>
    public required Money PaidAmount  { get; set; }
    
    public Money? RefundedAmount  { get; set; }
    
    /// <summary>
    ///     Gets or sets transaction status
    /// </summary>
    [Required, MaxLength(100)] public required PaymentStatus PaymentStatus { get; set; }
    
    /// <summary>
    /// Gets or sets the method used to make the payment (e.g., cash, check, credit card).
    /// </summary>
    [Required, MaxLength(100)] public required PaymentMethod PaymentMethod { get; set; }
    
    [MaxLength(255)] public string? PaymentId { get; set; }
    
    [MaxLength(255)] public string? Error { get; set; }
    
    /// <summary>
    ///     Gets or sets Church identifier
    /// </summary>
    public int? ChurchId { get; set; }
    
    /// <summary>
    /// Gets or sets the identifier of the benefactor who made the contribution.
    /// </summary>
    public int? BenefactorId { get; set; }
}