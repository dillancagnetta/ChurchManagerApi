using System.Security.Cryptography;
using System.Web;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Finances;
using ChurchManager.Domain.Features.Finances.Services;

namespace Payments.PayFast;

public class PayFastPaymentProvider(PayFastSettings settings) : IPaymentProvider
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

    #endregion

    #region Private Methods

    private string GenerateSignature(Dictionary<string, string> data, string passphrase = "")
    {
        var sortedData = data.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        var queryString = string.Join("&", sortedData.Select(kvp => $"{kvp.Key}={HttpUtility.UrlEncode(kvp.Value)}"));
            
        if (!string.IsNullOrEmpty(passphrase))
        {
            queryString += $"&passphrase={HttpUtility.UrlEncode(passphrase)}";
        }
            
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(queryString));
        return Convert.ToHexString(hash).ToLower();
    }

    #endregion

    #region IProvider Members

    public string ConfigurationUrl => PayFastPluginDefaults.ConfigurationUrl;
    public string SystemName  => PayFastPluginDefaults.ProviderSystemName;
    public string FriendlyName  => PayFastPluginDefaults.FriendlyName;
    public int Priority  => 1;

    #endregion
}