using ChurchManager.Domain.Common;

namespace ChurchManager.Domain.Features.Finances.Services;

/// <summary>
///     Provides an interface for creating payment gateways & methods
/// </summary>
public interface IPaymentProvider
{
    /// <summary>
    ///     Init a process a payment transaction
    /// </summary>
    /// <returns>Payment transaction</returns>
    Task<PaymentTransaction> InitPaymentTransactionAsync(CancellationToken ct = default);
    
    /// <summary>
    ///     Process a payment
    /// </summary>
    /// <returns>Process payment result</returns>
    Task<ProcessPaymentResult> ProcessPaymentAsync(PaymentTransaction payment, CancellationToken ct = default);
    
    /// <summary>
    ///     Post process payment
    /// </summary>
    Task PostProcessPaymentAsync(PaymentTransaction payment, CancellationToken ct = default);
    
    /// <summary>
    ///     Post redirect payment (used by payment gateways that redirecting to a another URL)
    /// </summary>
    Task<string> PostRedirectPaymentAsync(PaymentTransaction payment, CancellationToken ct = default);
    
    /// <summary>
    ///     Refunds a payment
    /// </summary>
    /// <returns>Result</returns>
    Task<RefundPaymentResult> RefundAsync(
        PaymentTransaction payment, Money amountToRefund, bool isPartialRefund = false, CancellationToken ct = default);
    
    /// <summary>
    ///     Voids a payment
    /// </summary>
    /// <returns>Result</returns>
    Task<VoidPaymentResult> VoidAsync(PaymentTransaction payment, CancellationToken ct = default);

    /// <summary>
    ///     Cancel payment transaction
    /// </summary>
    Task CancelPaymentAsync(PaymentTransaction payment, CancellationToken ct = default);
}