using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Features.Finances.Services;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Features.Finances.Services;

public class PaymentService(
    IEnumerable<IPaymentProvider> paymentProviders,
    ILogger<PaymentService> logger) : IPaymentService
{
    public Task<IList<IPaymentProvider>> LoadAllPaymentMethodsAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public IPaymentProvider LoadPaymentMethodBySystemName(string systemName)
    {
        throw new NotImplementedException();
    }
}