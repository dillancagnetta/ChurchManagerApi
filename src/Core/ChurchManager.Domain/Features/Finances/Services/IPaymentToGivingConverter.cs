namespace ChurchManager.Domain.Features.Finances.Services;

public interface IPaymentToGivingConverter
{
    Task<List<Giving>> ConvertCompletedPaymentsToGivingAsync(int? churchId = null, CancellationToken ct = default);
    Task<Giving?> ConvertPaymentToGivingAsync(int paymentId, CancellationToken ct = default);
    Task<bool> IsPaymentAlreadyInBankStatementAsync(PaymentTransaction payment, CancellationToken ct = default);
}