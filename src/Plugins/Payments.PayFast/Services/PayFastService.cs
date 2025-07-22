using System.Globalization;
using System.Security.Cryptography;
using System.Web;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features.Finances;
using ChurchManager.Domain.Features.Finances.Extensions;
using ChurchManager.Infrastructure.Abstractions.MultiTenancy;

namespace Payments.PayFast.Services;

public class PayFastService(
    PayFastSettings settings, 
    HttpClient httpClient,
    ITenantUrlResolver urlResolver) : IPayFastService
{
    public async Task<string> InitiatePaymentAsync(PaymentTransaction payment, CancellationToken ct)
    {
        var parsed = payment.PaymentReference!.ParsePaymentReference();
        // Must appear im this order for signature
        var data = new Dictionary<string, string?>
        {
            {"merchant_id", settings.MerchantId},
            {"merchant_key", settings.MerchantKey},
            {"return_url", urlResolver.CurrentTenantSubDomainUrl(settings.ReturnUrl)},
            {"cancel_url", urlResolver.CurrentTenantSubDomainUrl(settings.CancelUrl)},
            {"notify_url", urlResolver.CurrentTenantWebhookUrl(settings.NotifyUrl)},
            {"name_first", payment.FirstName},
            {"name_last", payment.LastName},
            {"email_address", string.Empty},
            {"cell_number", parsed.PhoneNumber?.FixZeroPhoneNumber()},
            {"m_payment_id", payment.PaymentId!},
            {"amount", payment.PaidAmount!.Amount.ToString(CultureInfo.InvariantCulture)},
            {"item_name", payment.PaymentReference!},
            {"item_description", payment.Description!}
        };
        
        var filteredData = data
            .Where(kvp => !string.IsNullOrEmpty(kvp.Value))
            .ToDictionary();
        
        var signature = GenerateSignature(filteredData!, settings.Passphrase);
        data.Add("signature", signature);
            
        var formContent = new FormUrlEncodedContent(filteredData);
        var queryString = await formContent.ReadAsStringAsync(ct);
        
        return $"{settings.PaymentUrl}?{queryString}"; 
    }

    public async Task<bool> ValidatePaymentAsync(Dictionary<string, string> postData, CancellationToken ct)
    {
        var tempData = new Dictionary<string, string>(postData);
        tempData.Remove("signature");
            
        var serverSignature = GenerateSignature(tempData, settings.Passphrase);
            
        if (serverSignature != postData.GetValueOrDefault("signature"))
        {
            return false;
        }
            
        // Validate with PayFast server
        var formContent = new FormUrlEncodedContent(postData);
        var response = await httpClient.PostAsync(settings.ValidateUrl, formContent, ct);
        var responseContent = await response.Content.ReadAsStringAsync(ct);
            
        return responseContent.Contains("VALID");
    }

    /// <summary>
    /// Concatenation of the name value pairs of all the non-blank variables with ‘&’ used as a separator
    /// The pairs must be listed in the order in which they appear in the attributes description.
    /// The passphrase is an extra security feature
    /// </summary>
    /// <param name="data"></param>
    /// <param name="passphrase"></param>
    /// <returns></returns>
    public string GenerateSignature(Dictionary<string, string> data, string? passphrase)
    {
        var queryString = string.Join("&", data.Select(kvp => $"{kvp.Key}={HttpUtility.UrlEncode(kvp.Value)}"));
            
        if (!string.IsNullOrEmpty(passphrase))
        {
            queryString += $"&passphrase={HttpUtility.UrlEncode(passphrase)}";
        }
            
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(queryString));
        return Convert.ToHexString(hash).ToLower();
    }
}