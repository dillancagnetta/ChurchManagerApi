using ChurchManager.Domain.Features.Finances;
using ChurchManager.Domain.Features.Finances.Services;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using CodeBoss.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Features.Finances.Services;

public class PaymentToGivingConverter(
    IGenericDbRepository<PaymentTransaction> paymentDb,
    IGenericDbRepository<Giving> givingDb,
    IGivingReferenceResolver givingReferenceResolver,
    ILogger<PaymentToGivingConverter> logger) : IPaymentToGivingConverter
{
    public async Task<List<Giving>> ConvertCompletedPaymentsToGivingAsync(int? churchId = null, CancellationToken ct = default)
    {
        var query = paymentDb.Queryable()
            .Where(p => p.Status == PaymentStatus.Completed)
            .Where(p => !p.ConvertedToGiving)
            .Where(p => p.PaymentMethodSystemName == "Payments.PayFast")
            ;

        if (churchId.HasValue)
        {
            query = query.Where(p => p.ChurchId == churchId.Value);
        }

        var completedPayments = await query.ToListAsync(ct);
        var createdGivings = new List<Giving>();

        foreach (var payment in completedPayments)
        {
            try
            {
                // Check if this payment already exists in bank statements
                if (await IsPaymentAlreadyInBankStatementAsync(payment, ct))
                {
                    logger.LogInformation($"Payment {payment.PaymentReference} already exists in bank statements, skipping conversion");
                    continue;
                }

                var giving = await ConvertPaymentToGivingInternalAsync(payment);
                if (giving != null)
                {
                    createdGivings.Add(giving);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to convert payment {payment.Id} to giving");
            }
        }

        return createdGivings;
    }

    public Task<Giving?> ConvertPaymentToGivingAsync(int paymentId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsPaymentAlreadyInBankStatementAsync(PaymentTransaction payment, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
    
    private async Task<Giving?> ConvertPaymentToGivingInternalAsync(PaymentTransaction payment)
    {
        if (payment.PaidAmount == null || string.IsNullOrEmpty(payment.PaymentReference))
        {
            logger.LogWarning($"Cannot convert payment {payment.Id}: missing amount, benefactor, or payment reference");
            return null;
        }

        // Use your existing giving reference resolver to extract fund, giving type, etc.
        var (giving, isMatched, error) = await givingReferenceResolver.ResolveToGivingAsync(payment.PaymentReference, payment.PaidAmount, payment.PaymentMethod);
        if (giving is null || !error.IsNullOrEmpty())
        {
            logger.LogWarning($"Could not resolve giving reference for payment {payment.Id} with reference {payment.PaymentReference}. Error: {error}");
            return null;
        }
        
        // Add the giving 
        await givingDb.AddAsync(giving);
        await givingDb.SaveChangesAsync();

        // Update payment transaction
        payment.ConvertedToGiving = true;
        payment.GivingId = giving.Id;
        payment.ChurchId = giving.ChurchId;
        await paymentDb.UpdateAsync(payment);
        await paymentDb.SaveChangesAsync();
        
        logger.LogInformation($"Converted payment {payment.Id} to giving {giving.Id}");
        return giving;
    }
}