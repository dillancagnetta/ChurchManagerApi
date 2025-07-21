using System.Globalization;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Finances;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payments.PayFast.Services;

namespace Payments.PayFast.Controllers;

[ApiController]
[Route($"{PayFastPluginDefaults.BaseUrl}")]
public class PayFastController(
    IPayFastService service, 
    IGenericDbRepository<PaymentTransaction> paymentsDb,
    PayFastSettings settings) : ControllerBase
{
    [HttpGet]
    public IActionResult HealthCheck()
    {
        return Ok($"All good in {PayFastPluginDefaults.FriendlyName} hood!");
    }

    [HttpPost("notify")]
    public async Task<IActionResult> PaymentNotify(CancellationToken ct)
    {
        var postData = new Dictionary<string, string>();

        foreach (var key in Request.Form.Keys)
        {
            postData[key] = Request.Form[key];
        }

        var isValid = await service.ValidatePaymentAsync(postData, ct);

        if (isValid)
        {
            var paymentId = postData.GetValueOrDefault("m_payment_id");

            var payment = await paymentsDb.Queryable().FirstOrDefaultAsync(p => p.PaymentId == paymentId, ct)
                          ?? throw new InvalidOperationException($"Payment not found: {paymentId}");

            payment.Status = postData.GetValueOrDefault("payment_status") switch
            {
                "COMPLETE" => PaymentStatus.Completed,
                "FAILED" => PaymentStatus.Failed,
                "CANCELLED" => PaymentStatus.Cancelled,
                _ => PaymentStatus.Processing
            };

            payment.ExternalReferenceId = postData.GetValueOrDefault("pf_payment_id");
            //payment.PaymentMethod = postData.GetValueOrDefault("payment_method");

            if (decimal.TryParse(postData.GetValueOrDefault("amount_fee"), NumberStyles.Number, CultureInfo.InvariantCulture,out var fee))
                payment.Fee = new Money(Currency.ZAR, Math.Abs(fee)); // make it positive amount

            if (decimal.TryParse(postData.GetValueOrDefault("amount_net"),NumberStyles.Number, CultureInfo.InvariantCulture, out var netAmount))
                payment.NetAmount = new Money(Currency.ZAR, netAmount);

            if (payment.Status == PaymentStatus.Completed)
            {
                payment.CompletedDate = DateTime.UtcNow;
            }
            else if (payment.Status == PaymentStatus.Failed)
            {
                payment.Error = postData.GetValueOrDefault("failure_reason");
            }

            await paymentsDb.SaveChangesAsync(ct);
            
            return Ok();
        }

        return Problem();
    }
}