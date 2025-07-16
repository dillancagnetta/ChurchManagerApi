using ChurchManager.Domain.Common;
using Codeboss.Results;

namespace ChurchManager.Domain.Features.Finances;

public class ProcessPaymentResult : OperationResult<ProcessPaymentState>;
public class RefundPaymentResult : OperationResult<ProcessPaymentState>;
public class VoidPaymentResult : OperationResult<ProcessPaymentState>;

public record ProcessPaymentState
{
    /// <summary>
    ///     Gets or sets a payment transaction status after processing
    /// </summary>
    public PaymentStatus NewPaymentPaymentStatus { get; set; } = PaymentStatus.Pending;
    
    /// <summary>
    ///     Gets or sets value paid amount
    /// </summary>
    public Money? PaidAmount { get; set; }
}