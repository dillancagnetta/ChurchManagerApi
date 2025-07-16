using ChurchManager.Domain.Features.Settings;

namespace Payments.PayFast;

public class PayFastSettings : ISettings
{
    public string MerchantId { get; set; } = string.Empty;
    public string MerchantKey { get; set; } = string.Empty;
    public string? Passphrase { get; set; }
    public bool TestMode { get; set; } = true;
    public string BaseUrl => TestMode ? "https://sandbox.payfast.co.za" : "https://www.payfast.co.za";
    public string PaymentUrl => $"{BaseUrl}/eng/process";
    public string ValidateUrl => $"{BaseUrl}/eng/query/validate";
}