using ChurchManager.Domain.Features.Finances;
using ChurchManager.Domain.Features.Finances.Services;

namespace ChurchManager.Application.Abstractions.Services;

/// <summary>
///     Load active payment methods
/// </summary>
public interface IPaymentService
{
    /// <summary>
    ///     Load active payment methods
    /// </summary>
    Task<IEnumerable<IPaymentProvider>> LoadAllPaymentMethodsAsync(CancellationToken ct);
    
    /// <summary>
    ///     Load payment provider by system name
    /// </summary>
    IPaymentProvider? LoadPaymentMethodBySystemName(string systemName);
    
    /// <summary>
    ///     Post redirect payment (used by payment gateways that redirecting to a another URL)
    /// </summary>
    /// <param name="payment">Payment transaction</param>
    /// <param name="ct">CancellationToken</param>
    Task<string?> PostRedirectPaymentAsync(PaymentTransaction payment, CancellationToken ct);
}