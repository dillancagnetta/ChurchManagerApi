using ChurchManager.Domain.Features.Finances.Services;

namespace ChurchManager.Application.Abstractions.Services;

/// <summary>
///     Load active payment methods
/// </summary>
public interface IPaymentService
{
    Task<IList<IPaymentProvider>> LoadAllPaymentMethodsAsync(CancellationToken ct);
    IPaymentProvider LoadPaymentMethodBySystemName(string systemName);
}