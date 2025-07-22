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
    /// This will process transactions in the imported bank statement and resolve the giving references
    /// So it will try to match to the giving church and person or family (via phone number)
    /// If fully matched: (to church and phone number) it will process the transaction and create matching giving record
    /// If not fully matched: the transaction will be marked as unprocessed
    /// 
    /// *** for now we only support Person and Family giving ***
    /// 
    ///Example formats:
    ///     Person: CHU-082xxxxxxx-T
    ///     Family: CHU-082xxxxxxx-P-HS-F
    /// </summary>
    public async Task<BankStatementProcessResult> ResolveAsync(BankStatementImport import, CancellationToken ct =  default)
    {
        // People map for faster lookups
        var peoplePhoneMap =
            await peopleDb.FindPhoneNumberForPeople(ParsePhoneNumbers(import.OriginalTransactionReferences()), ct);

        // TODO: Create other reference matchers e.g. for Churches that give
        try
        {
            foreach (var transaction in import.Transactions)
            {
                try
                {
                    transaction.Import = import;
                    
                    var reference = transaction.OriginalReference;

                    if (!FinancesExtensions.IsValidPaymentReference(reference))
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
                    resolvedReference = await TryResolveChurchAsync(reference, resolvedReference, ct);
                    resolvedReference = await TryResolvePersonAsync(reference, resolvedReference, peoplePhoneMap, ct);

                    transaction.IsMatched = resolvedReference.IsPersonMatched && resolvedReference.IsChurchMatched;
                    
                    if (transaction.IsMatched)
                    {
                        var fund = await ResolveFundAsync(resolvedReference.GivingType, parsed.PartnershipFund!, ct);
                        var benefactor = await ResolveBenefactorAsync(resolvedReference, ct);

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

    private async Task<Benefactor> ResolveBenefactorAsync(GivingReference reference, CancellationToken ct =  default)
    {
        Benefactor benefactor;
        var (_, phoneNumber, _, _, _) = FinancesExtensions.ParsePaymentReference(reference.OriginalReference!);
        switch (reference.BenefactorType.Value)
        {
            case "Individual":
                benefactor = await benefactorsDb.Queryable()
                                 .FirstOrDefaultAsync(x => x.PersonId == reference.Person!.Id, ct)
                             ?? new Benefactor
                             {
                                 Name = reference.Person!.Name, PersonId = reference.Person!.Id,
                                 Type = BenefactorType.Individual,
                                 PhoneNumber = phoneNumber
                             };

                return benefactor;
            case "Family":
                benefactor = await benefactorsDb.Queryable()
                                 .FirstOrDefaultAsync(x => x.FamilyId == reference.Family!.Id, ct)
                             ?? new Benefactor
                             {
                                 Name = reference.Family!.Name, FamilyId = reference.Family!.Id,
                                 Type = BenefactorType.Family,
                                 PhoneNumber = phoneNumber
                             };
                return benefactor;
            // Not used for now
            case "Church":
                benefactor = await benefactorsDb.Queryable()
                                 .FirstOrDefaultAsync(x => x.ChurchId == reference.Church!.Id, ct)
                             ?? new Benefactor
                             {
                                 Name = reference.Church!.Name, ChurchId = reference.Church!.Id,
                                 Type = BenefactorType.Church,
                                 PhoneNumber = phoneNumber
                             };
                return benefactor;
            // Not used for now
            case "External":
               
                return new Benefactor
                {
                    Name = BenefactorType.External.Value,
                    Type = BenefactorType.External,
                    PhoneNumber = phoneNumber
                };

            default:
                throw new ArgumentOutOfRangeException(nameof(reference.BenefactorType), reference.BenefactorType, "Invalid benefactor type");
        }
    }

    /// <summary>
    /// Resolve the church using the church short code.
    /// </summary>
    public async Task<GivingReference> TryResolveChurchAsync(string reference, GivingReference resolvedReference, CancellationToken ct =  default)
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
        Dictionary<string, Person?> map, CancellationToken ct =  default)
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

    public async Task<GivingReference> TryResolvePersonAsync(string reference, CancellationToken ct)
    {
        var (_, phoneNumber, _, _, isFamily) = FinancesExtensions.ParsePaymentReference(reference);
        var person = await peopleDb.FindBasicPersonByPhoneNumberAsync(phoneNumber!, ct);

        var givingReference = new GivingReference();
        if (person is not null)
        {
            givingReference.IsPersonMatched = true;
            givingReference.Person = new BeneficiaryInfo(person.PersonId, $"{person.FirstName} {person.LastName}");
        }
        
        return givingReference;
    }

   

    public IList<string> ParsePhoneNumbers(IList<string> references)
    {
        return references
            .Where(x => FinancesExtensions.IsValidPaymentReference(x))
            .Select(x => FinancesExtensions.ParsePaymentReference(x))
            .Select(x => x.PhoneNumber)
            .Where(x => !x.IsNullOrEmpty())
            .ToList();
    }
    
    public async Task<Fund> ResolveFundAsync(GivingType fundCode, string partnershipFund, CancellationToken ct =  default)
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

            if (!FinancesExtensions.IsValidPaymentReference(reference))
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