using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Common;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Finances;

public class Giving: AuditableEntity<int>, IAggregateRoot<int>
{
    public int BenefactorId { get; set; }
    public DateTime Date { get; private set; }
    public required Money Amount  { get; set; }   
    [Required] public required PaymentMethod PaymentMethod { get; set; }
    [Required] public required GivingType GivingType { get; set; }
    public int FundId { get; private set; }
    [MaxLength(255)] public string? Notes { get; private set; }
    [MaxLength(255)] public string? ExternalReferenceId { get; private set; }
    public bool ReceiptSent { get; private set; }

    #region Navigation properties

    public Benefactor? Benefactor { get; set; }
    public Fund? Fund { get; set; }

    #endregion
}