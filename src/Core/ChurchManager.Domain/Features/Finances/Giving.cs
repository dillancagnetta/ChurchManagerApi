using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Finances.Banking;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Finances;

/// <summary>
/// Represents a financial contribution or donation made to the church.
/// This class serves as an aggregate root for tracking giving transactions.
/// </summary>
/// <remarks>
/// The Giving entity captures all details related to financial contributions,
/// including who gave (Benefactor), how much (Amount), when (Date), and for what purpose (Fund).
/// </remarks>
public class Giving: AuditableEntity<int>, IAggregateRoot<int>
{
    /// <summary>
    /// Gets or sets the identifier of the benefactor who made the contribution.
    /// </summary>
    public int BenefactorId { get; set; }
    
    /// <summary>
    /// Gets the date when the contribution was made.
    /// </summary>
    public DateTime Date { get; set; }
    
    /// <summary>
    /// Gets or sets the monetary amount of the contribution.
    /// </summary>
    public required Money GivingAmount  { get; set; }
    
    /// <summary>
    /// Gets or sets the method used to make the payment (e.g., cash, check, credit card).
    /// </summary>
    [Required, MaxLength(100)] public required PaymentMethod PaymentMethod { get; set; }
    
    /// <summary>
    /// Gets or sets the type of giving (e.g., tithe, offering, special collection).
    /// </summary>
    [Required, MaxLength(100)] public required GivingType GivingType { get; set; }
    
    /// <summary>
    /// Gets the identifier of the fund to which the contribution is allocated.
    /// </summary>
    public int FundId { get; set; }
    
    /// <summary>
    /// Gets any additional notes or comments related to the contribution.
    /// </summary>
    [MaxLength(255)] public string? Notes { get; set; }
    
    /// <summary>
    /// Gets an external reference identifier for the contribution, such as a transaction ID from a payment processor.
    /// </summary>
    [MaxLength(255)] public string? ExternalReferenceId { get; set; }
    
    /// <summary>
    /// Gets a value indicating whether a receipt has been sent to the benefactor for this contribution.
    /// </summary>
    public bool ReceiptSent { get; set; }
    
    /// <summary>
    /// The Church the giving is originated from.
    /// </summary>
    public int? ChurchId { get; set; }

    #region Import Properties

    /// <summary>
    /// Gets the standardized payment reference used for matching (e.g., "GRE-0825432341-T")
    /// </summary>
    [MaxLength(50)] public string? ParsedReference { get; set; }
    
    /// <summary>
    /// Gets the original bank transaction ID for duplicate detection
    /// </summary>
    [MaxLength(255)] public string? BankTransactionId { get; set; }
    
    /// <summary>
    /// Gets the import batch this giving record came from
    /// </summary>
    public int? BankStatementImportId { get; set; }
    
    #endregion
    
    #region Navigation properties

    /// <summary>
    /// Gets or sets the benefactor entity associated with this contribution.
    /// </summary>
    public virtual Benefactor? Benefactor { get; set; }
    
    /// <summary>
    /// Gets or sets the fund entity to which this contribution is allocated.
    /// </summary>
    public virtual Fund? Fund { get; set; }
    
    /// <summary>
    /// Gets or sets the import batch entity this giving came from
    /// </summary>
    public virtual BankStatementImport? Import { get; set; }
    
    public virtual Church? Church { get; set; }

    #endregion

    public static Giving Create(Transaction transaction, GivingReference reference, Fund fund, Benefactor benefactor, string? notes)
    {
        return new Giving
        {
            //Import = import,
            GivingAmount = new Money(transaction.TransactionAmount),
            Date = transaction.TransactionDate,
            PaymentMethod = PaymentMethod.FromTransactionType(transaction.TransactionType),
            GivingType = reference.GivingType,
            FundId = fund.Id,
            BenefactorId = benefactor.Id,
            BankTransactionId = transaction.BankTransactionId,
            ParsedReference = transaction.OriginalReference,
            ChurchId = reference.Church?.Id, // Should be resolved from payment reference
            Notes = notes,
        };
    }
}