using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Finances;
using ChurchManager.Domain.Features.Finances.Services;

namespace Payments.PayFast;

public class PayFastPaymentProvider : IPaymentProvider
{
    public Task<PaymentTransaction> InitPaymentTransactionAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<ProcessPaymentResult> ProcessPaymentAsync(PaymentTransaction payment, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task PostProcessPaymentAsync(PaymentTransaction payment, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<string> PostRedirectPaymentAsync(PaymentTransaction payment, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RefundPaymentResult> RefundAsync(PaymentTransaction payment, Money amountToRefund, bool isPartialRefund = false,
        CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<VoidPaymentResult> VoidAsync(PaymentTransaction payment, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task CancelPaymentAsync(PaymentTransaction payment, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}