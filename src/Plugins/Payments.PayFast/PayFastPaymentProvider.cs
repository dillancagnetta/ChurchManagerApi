using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Finances;
using ChurchManager.Domain.Features.Finances.Services;
using Payments.PayFast.Services;

namespace Payments.PayFast;

public class PayFastPaymentProvider(IPayFastService service) : IPaymentProvider
{
    #region IPaymentProvider Members

    public Task<PaymentTransaction> InitPaymentTransactionAsync(CancellationToken ct = default)
    {
        var transaction = new PaymentTransaction
        {
            PaymentMethodSystemName = PayFastPluginDefaults.ProviderSystemName,
            PaidAmount = new Money(Currency.ZAR, 0),
            PaymentMethod = PaymentMethod.Unknown,
        };
        
        return Task.FromResult(transaction);
    }

    public Task<ProcessPaymentResult> ProcessPaymentAsync(PaymentTransaction payment, CancellationToken ct = default)
    {
        // Not used
        var result = new ProcessPaymentResult(new ProcessPaymentState
        {
            NewPaymentPaymentStatus = PaymentStatus.Pending,
        });
        
        return Task.FromResult(result);
    }

    public Task PostProcessPaymentAsync(PaymentTransaction payment, CancellationToken ct = default)
    {
        //nothing
        return Task.CompletedTask;
    }

    public Task<string> PostRedirectPaymentAsync(PaymentTransaction payment, CancellationToken ct = default)
    {
        return service.InitiatePaymentAsync(payment, ct);
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

    #endregion

    #region IProvider Members

    public string ConfigurationUrl => PayFastPluginDefaults.ConfigurationUrl;
    public string SystemName  => PayFastPluginDefaults.ProviderSystemName;
    public string FriendlyName  => PayFastPluginDefaults.FriendlyName;
    public int Priority  => 1;

    #endregion
}