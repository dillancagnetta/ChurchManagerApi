using ChurchManager.Domain.Features.Finances;

namespace Payments.PayFast.Services;

public interface IPayFastService
{
    Task<string> InitiatePaymentAsync(PaymentTransaction payment, CancellationToken ct);

    Task<bool> ValidatePaymentAsync(Dictionary<string, string> postData, CancellationToken ct);
    
    string GenerateSignature(Dictionary<string, string> data, string? passphrase);
}