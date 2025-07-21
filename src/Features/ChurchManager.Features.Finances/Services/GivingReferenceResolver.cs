using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Finances;
using ChurchManager.Domain.Features.Finances.Banking;
using ChurchManager.Domain.Features.Finances.Extensions;
using ChurchManager.Domain.Features.Finances.Services;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using CodeBoss.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.Finances.Services;

public class GivingReferenceResolver(
    IReadDbRepository<Church> churchesDb,
    IPersonDbRepository peopleDb,
    IReadDbRepository<Fund> fundsDb,
    IReadDbRepository<Benefactor> benefactorsDb,
    IQueryCache cache
) : IGivingReferenceResolver
{
   

    /// <summary>
    ///Example formats:
    ///  Person: CHU-082xxxxxxx-T
    ///  Family: CHU-082xxxxxxx-P-HS-F
    /// </summary>
    public async Task<BankStatementProcessResult> ResolveAsync(BankStatementImport import)
    {
        var peoplePhoneMap =
            await peopleDb.FindPhoneNumberForPeople(ParsePhoneNumbers(import.OriginalTransactionReferences()));

        try
        {
            foreach (var transaction in import.Transactions)
            {
                try
                {
                    transaction.Import = import;
                    
                    var reference = transaction.OriginalReference;

                    if (!FinancesExtensions.IsValidReference(reference))
                    {
                        transaction.SetAsUnProcessed("Invalid reference format");
                        continue;
                    }

                    // Its safe to continue processing the reference
                    var parsed = FinancesExtensions.ParsePaymentReference(reference);

                    var resolvedReference = new GivingReference
                    {
                        GivingType = parsed.Type,
                        BenefactorType = parsed.IsFamily ? BenefactorType.Family : BenefactorType.Individual,
                        OriginalReference = reference,
                    };

                    // Try to resolve the church and the person
                    resolvedReference = await TryResolveChurchAsync(reference, resolvedReference);
                    resolvedReference = await TryResolvePersonAsync(reference, resolvedReference, peoplePhoneMap);

                    transaction.IsMatched = resolvedReference.IsPersonMatched && resolvedReference.IsChurchMatched;

                    if (transaction.IsMatched)
                    {
                        var fund = await ResolveFundAsync(resolvedReference.GivingType, parsed.PartnershipFund!);
                        var benefactor = await ResolveBenefactorAsync(resolvedReference);

                        // Create associated giving record
                        import.AddGiving(Giving.Create(transaction, resolvedReference, fund, benefactor, transaction.Memo));
                        transaction.SetAsProcessed();
                    }
                    else 
                    {
                        transaction.SetAsUnProcessed("Unable to resolve church and phone number");
                        continue;
                    }
                }
                catch (Exception e)
                {
                    transaction.SetAsUnProcessed(e.Message);
                }
            }
            
            return new BankStatementProcessResult(import);
        }
        catch (Exception e)
        {
            import.AddError(e.Message);
            return new BankStatementProcessResult(import) { IsSuccess = false };
        }
    }

    private async Task<Benefactor> ResolveBenefactorAsync(GivingReference reference)
    {
        //var benefactor = Benefactor.FromGivingReference(reference);

        Benefactor benefactor;

        switch (reference.BenefactorType.Value)
        {
            case "Individual":
                benefactor = await benefactorsDb.Queryable()
                                 .FirstOrDefaultAsync(x => x.PersonId == reference.Person!.Id)
                             ?? new Benefactor
                             {
                                 Name = reference.Person!.Name, PersonId = reference.Person!.Id,
                                 Type = BenefactorType.Individual
                             };

                return benefactor;
            case "Family":
                benefactor = await benefactorsDb.Queryable()
                                 .FirstOrDefaultAsync(x => x.FamilyId == reference.Family!.Id)
                             ?? new Benefactor
                             {
                                 Name = reference.Family!.Name, PersonId = reference.Family!.Id,
                                 Type = BenefactorType.Family
                             };
                return benefactor;
            case "Church":
                benefactor = await benefactorsDb.Queryable()
                                 .FirstOrDefaultAsync(x => x.ChurchId == reference.Church!.Id)
                             ?? new Benefactor
                             {
                                 Name = reference.Church!.Name, PersonId = reference.Church!.Id,
                                 Type = BenefactorType.Church
                             };
                return benefactor;

            default:
                throw new ArgumentOutOfRangeException(nameof(reference.BenefactorType), reference.BenefactorType, "Invalid benefactor type");
        }
    }

    /// <summary>
    /// Resolve the church using the church short code.
    /// </summary>
    public async Task<GivingReference> TryResolveChurchAsync(string reference, GivingReference resolvedReference)
    {
        var (churchCode, _, _, _, _) = FinancesExtensions.ParsePaymentReference(reference);

        var churches = await cache.GetOrSetAsync("churches", () => churchesDb.Queryable().AsNoTracking().Select(x => new
        {
            x.ShortCode,
            x.Id,
            x.Name
        }).ToListAsync());
        var church = churches.FirstOrDefault(x => x.ShortCode == churchCode);

        var foundChurch = church is not null;
        resolvedReference.IsChurchMatched = foundChurch;
        resolvedReference.Church = foundChurch ? new BeneficiaryInfo(church!.Id, church!.Name) : null;

        return resolvedReference;
    }

    /// <summary>
    /// Resolve the person using the phone number.
    /// </summary>
    public Task<GivingReference> TryResolvePersonAsync(string reference, GivingReference resolvedReference,
        Dictionary<string, Person?> map)
    {
        var (_, phoneNumber, _, _, isFamily) = FinancesExtensions.ParsePaymentReference(reference);
        var person = map.GetValueOrDefault(phoneNumber);

        var foundPerson = person is not null;
        resolvedReference.IsPersonMatched = foundPerson;
        resolvedReference.Person = foundPerson ? new BeneficiaryInfo(person!.Id, person!.FullName!.ToString()) : null;
        resolvedReference.Family = isFamily && foundPerson
            ? new BeneficiaryInfo(person!.FamilyId!.Value, person!.Family!.Name!)
            : null;

        return Task.FromResult(resolvedReference);
    }

    public async Task<PersonViewModelBasic?> TryResolvePersonAsync(string reference, CancellationToken ct)
    {
        var (_, phoneNumber, _, _, isFamily) = FinancesExtensions.ParsePaymentReference(reference);
        var person = await peopleDb.FindBasicPersonByPhoneNumberAsync(phoneNumber!, ct);
        return person;
    }

    public IList<string> ParsePhoneNumbers(IList<string> references)
    {
        return references
            .Where(x => FinancesExtensions.IsValidReference(x))
            .Select(x => FinancesExtensions.ParsePaymentReference(x))
            .Select(x => x.PhoneNumber)
            .Where(x => !x.IsNullOrEmpty())
            .ToList();
    }
    
    public async Task<Fund> ResolveFundAsync(GivingType fundCode, string partnershipFund)
    {
        Fund fund;
        var funds = await cache.GetOrSetAsync("funds", () => fundsDb.Queryable().AsNoTracking().ToListAsync());

        var defaultFund = funds.First(x => x.IsSystem);

        if (fundCode == GivingType.Unknown) return defaultFund;

        // Get the sub fund if Parnership e.g. ROR
        if (fundCode == GivingType.Partnership)
        {
            // if we cant find the fund we use the Parent Partnership fund as a fallback
            fund = funds.FirstOrDefault(x => x.Code == partnershipFund) ?? funds.First(x => x.Code == "PARTNER");
            return fund;
        }

        // Tithes contains 'Tithe'
        fund = funds.FirstOrDefault(x => x.Name.Contains(fundCode.Value)) ?? defaultFund;

        return fund;
    }

    #region Private Methods

    /// <summary>
    /// Processes a single reference and returns the resolved giving reference with matching status.
    /// </summary>
    /// <param name="reference">The payment reference string to process</param>
    /// <param name="money"></param>
    /// <param name="paymentMethod"></param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>A tuple containing the resolved reference and whether it was successfully matched</returns>
    public async Task<(Giving? giving, bool IsMatched, string? ErrorMessage)> ResolveToGivingAsync(
        string reference, 
        Money money, 
        PaymentMethod paymentMethod,
        CancellationToken ct = default)
    {
        try
        {
            var peoplePhoneMap =
                await peopleDb.FindPhoneNumberForPeople(ParsePhoneNumbers([reference]), ct);

            if (!FinancesExtensions.IsValidReference(reference))
            {
                return (null, false, "Invalid reference format");
            }

            // Its safe to continue processing the reference
            var parsed = FinancesExtensions.ParsePaymentReference(reference);

            var resolvedReference = new GivingReference
            {
                GivingType = parsed.Type,
                BenefactorType = parsed.IsFamily ? BenefactorType.Family : BenefactorType.Individual,
                OriginalReference = reference,
            };

            // Try to resolve the church and the person
            resolvedReference = await TryResolveChurchAsync(reference, resolvedReference);
            resolvedReference = await TryResolvePersonAsync(reference, resolvedReference, peoplePhoneMap);

            var isMatched = resolvedReference.IsPersonMatched && resolvedReference.IsChurchMatched;

            if (!resolvedReference.IsPersonMatched)
            {
                resolvedReference.BenefactorType = BenefactorType.Unknown;
            }

            var fund = await ResolveFundAsync(resolvedReference.GivingType, parsed.PartnershipFund!);
            var benefactor = await ResolveBenefactorAsync(resolvedReference);
            
            // Create associated giving record
            var giving = Giving.Create(money, paymentMethod, resolvedReference, fund, benefactor);
            return (giving, isMatched, null);
        }
        catch (Exception e)
        {
            return (null, false, e.Message);
        }
    }

    #endregion
}