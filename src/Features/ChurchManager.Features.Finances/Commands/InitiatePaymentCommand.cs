using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Finances;
using ChurchManager.Domain.Features.Finances.Services;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using CodeBoss.Extensions;
using MediatR;

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
    IPaymentService service) : IRequestHandler<InitiatePaymentCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(InitiatePaymentCommand command, CancellationToken ct)
    {
        var amount = command.TestMode ? 1045.45m : command.Amount.Amount;
        var payment = new PaymentTransaction
        {
            PaidAmount = new Money(command.Amount.Currency ?? Currency.ZAR, amount),
            PaymentMethod = command.PaymentMethod,
            PaymentMethodSystemName = command.PaymentSystem,
            PaymentReference = command.PaymentReference,
            Description = command.TestMode ? "Test Transaction" : command.Description,
        };

        if (command.PersonId.HasValue)
        {
           var person = await personDb.BasicPersonViewModelAsync(command.PersonId.Value, ct)
                     ?? throw new NullReferenceException($"Unable to resolve person by PersonId: {command.PersonId} ");
           payment.PersonId = person.PersonId;
           payment.FirstName = person.FirstName;
           payment.LastName = person.LastName;
        }
        else
        {
            var person = await referenceResolver.TryResolvePersonAsync(command.PaymentReference, ct);

            if (person is null) // Still unable to resolve person
            {
                if (command.FirstName.IsNullOrEmpty() && command.LastName.IsNullOrEmpty())
                {
                    throw new NullReferenceException($"Missing first name and last name for payment reference: {command.PaymentReference}");
                }
                
                payment.FirstName = command.FirstName;
                payment.LastName = command.LastName;
            }
            else
            {
                payment.PersonId = person.PersonId;
                payment.FirstName = person.FirstName;
                payment.LastName = person.LastName; 
            }
        }
        
        await paymentsDb.AddAsync(payment, ct);
        await paymentsDb.SaveChangesAsync(ct); 
        
        var paymentRedirectUrl = await service.PostRedirectPaymentAsync(payment, ct);
        
        return new ApiResponse(paymentRedirectUrl, "Success");
    }
}