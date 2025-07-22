using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Finances;
using ChurchManager.Domain.Features.Finances.Extensions;
using ChurchManager.Domain.Features.Finances.Services;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using CodeBoss.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Features.Finances.Commands;

public record InitiatePaymentCommand : IRequest<ApiResponse>
{
    public required MoneyViewModel Amount { get; set; }
    public int? PersonId { get; set; } // Person who is making the payment, optional
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string PaymentSystem { get; set; } 
    public required string PaymentReference { get; set; } 
    public string? Description { get; set; }
    public required string PaymentMethod { get; set; } 

    public bool TestMode { get; set; } = true;

};

public class InitiatePaymentHandler(
    IPersonDbRepository personDb,
    IGenericDbRepository<PaymentTransaction> paymentsDb,
    IGivingReferenceResolver referenceResolver,
    IPaymentService service,
    ILogger<InitiatePaymentHandler> logger) : IRequestHandler<InitiatePaymentCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(InitiatePaymentCommand command, CancellationToken ct)
    {
        // Invalid Payment Reference
        if (!command.PaymentReference.IsValidPaymentReference())
        {
            return ApiResponse.Failed("Invalid payment reference format");
        }
        
        var amount = command.TestMode ? 1045.45m : command.Amount.Amount;
        var payment = new PaymentTransaction
        {
            PaidAmount = new Money(command.Amount.Currency ?? Currency.ZAR, amount),
            PaymentMethod = command.PaymentMethod,
            PaymentMethodSystemName = command.PaymentSystem,
            PaymentReference = command.PaymentReference,
            Description = command.TestMode ? "Test Transaction" : command.Description,
            IsTest = command.TestMode,
        };

        // Try resolve person
        if (command.PersonId.HasValue)
        {
           var person = await personDb.BasicPersonViewModelAsync(command.PersonId.Value, ct)
                     ?? throw new NullReferenceException($"Unable to resolve person by PersonId: {command.PersonId} ");
           payment.PersonId = person.PersonId;
           payment.FirstName = person.FirstName;
           payment.LastName = person.LastName;
           logger.LogInformation("Resolved person with (PersonId: {PersonId})", command.PersonId.Value);
        }
        else
        {
            // Try from the phone number in reference
            var personGivingRef = await referenceResolver.TryResolvePersonAsync(command.PaymentReference, ct);

            if (!personGivingRef.IsPersonMatched) // Still unable to resolve person
            {
                logger.LogInformation("Cannot resolve person from phone number (reference: {reference})", command.PaymentReference);
                // Just use provided first name and last name if available
                if (command.FirstName.IsNullOrEmpty() && command.LastName.IsNullOrEmpty())
                {
                    throw new NullReferenceException($"Missing first name and last name for payment reference: {command.PaymentReference}");
                }
                
                payment.FirstName = command.FirstName;
                payment.LastName = command.LastName;
            }
            else
            {
                var names = personGivingRef.Person!.Name.Split(" ");
                payment.PersonId = personGivingRef.Person.Id;
                payment.FirstName = names![0];
                payment.LastName = names.Length > 1 ? names[1] : string.Empty; 
                logger.LogInformation("Resolved person from phone number (PersonId: {PersonId})", personGivingRef.Person.Id);
            }
        }
        
        // Try resolve church
        var churchGivingRef = await referenceResolver.TryResolveChurchAsync(payment.PaymentReference, new GivingReference(), ct);
        if (churchGivingRef.IsChurchMatched)
        {
            payment.ChurchId = churchGivingRef.Church!.Id;
            logger.LogInformation("Resolved church from short code (Reference: {Reference})", payment.PaymentReference);
        }
        else
        {
            logger.LogInformation("Could not resolved church from short code (Reference: {Reference})", payment.PaymentReference);
            //TODO: may set church from the resolved person if available
        }
        
        await paymentsDb.AddAsync(payment, ct);
        await paymentsDb.SaveChangesAsync(ct); 
        
        var paymentRedirectUrl = await service.PostRedirectPaymentAsync(payment, ct);
        
        return new ApiResponse(paymentRedirectUrl, "Success");
    }
}