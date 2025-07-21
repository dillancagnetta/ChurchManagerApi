using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.Finances;
using ChurchManager.Domain.Features.Finances.Services;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Features.Finances.Services;

public class PaymentService(
    IEnumerable<IPaymentProvider> paymentProviders,
    ILogger<PaymentService> logger) : IPaymentService
{
    public Task<IEnumerable<IPaymentProvider>> LoadAllPaymentMethodsAsync(CancellationToken ct)
    {
        return Task.FromResult(paymentProviders);
    }

    public IPaymentProvider? LoadPaymentMethodBySystemName(string systemName)
    {
        return paymentProviders?.Where(x => x.SystemName == systemName).FirstOrDefault();;
    }

    public async Task<string?> PostRedirectPaymentAsync(PaymentTransaction payment, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(payment);
        
        if (payment.Status == PaymentStatus.Completed) return string.Empty;
        var paymentMethod = LoadPaymentMethodBySystemName(payment.PaymentMethodSystemName);
        if (paymentMethod == null) throw new Exception("Payment method couldn't be loaded");
        
        return await paymentMethod.PostRedirectPaymentAsync(payment, ct);
    }
}