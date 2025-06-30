using System.Text.RegularExpressions;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.Finances;
using ChurchManager.Domain.Features.Finances.Banking;
using ChurchManager.Domain.Features.Finances.Services;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Repositories;
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
    /// Format: 3 letters - 10-digit number starting with 0 - at least one letter
    /// If the first letter after the number is P, there must be another letter segment (e.g., -P-HS)
    /// Final optional -F
    /// Example: CHU-0821234567-T
    /// </summary>
    const string ReferencePattern = @"^[A-Za-z]{3}-0\d{9}-(?!P$)(?:[A-Za-z]+(?:-[A-Za-z]+)*)(?:-F)?$";

    /// <summary>
    ///Example formats:
    ///  Person: CHU-082xxxxxxx-T
    ///  Family: CHU-082xxxxxxx-P-HS-F
    /// </summary>
    public async Task<BankStatementProcessResult> ResolveAsync(BankStatementImport import)
    {
        var peoplePhoneMap =
            await peopleDb.FindPhoneNumberForPeople(ParsePhoneNumbers(import.OriginalTransactionReferences()));

        foreach (var transaction in import.Transactions)
        {
            try
            {
                var reference = transaction.OriginalReference;

                if (!IsValidReference(reference))
                {
                    transaction.Error = "Invalid reference format";
                    transaction.SetAsUnProcessed();
                    continue;
                }

                // Its safe to continue processing the reference
                var parsed = Parse(reference);

                var resolvedReference = new GivingReference
                {
                    GivingType = parsed.Type,
                    BenefactorType = parsed.IsFamily ? BenefactorType.Family : BenefactorType.Individual,
                };

                // Try to resolve the church and the person
                resolvedReference = await TryResolveChurchAsync(reference, resolvedReference);
                resolvedReference = await TryResolvePersonAsync(reference, resolvedReference, peoplePhoneMap);

                transaction.IsMatched = resolvedReference.IsPersonMatched && resolvedReference.IsChurchMatched;

                if (transaction.IsMatched)
                {
                    var fund = await ResolveFundAsync(resolvedReference.GivingType, parsed.PartnershipFund!);
                    var benefactor = await ResolveBenefactorAsync(resolvedReference);

                    transaction.Giving = Giving.Create(transaction, resolvedReference, fund, benefactor, transaction.Memo);
                    transaction.SetAsProcessed();
                }
                else 
                {
                    transaction.Error = "Unable to resolve church and phone number";
                    transaction.SetAsUnProcessed();
                    continue;
                }
            }
            catch (Exception e)
            {
                transaction.Error = e.Message;
                transaction.SetAsUnProcessed();
            }
        }

        return new BankStatementProcessResult(import);
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
        var (churchCode, _, _, _, _) = Parse(reference);

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
        var (_, phoneNumber, _, _, isFamily) = Parse(reference);
        var person = map.GetValueOrDefault(phoneNumber);

        var foundPerson = person is not null;
        resolvedReference.IsPersonMatched = foundPerson;
        resolvedReference.Person = foundPerson ? new BeneficiaryInfo(person!.Id, person!.FullName!.ToString()) : null;
        resolvedReference.Family = isFamily && foundPerson
            ? new BeneficiaryInfo(person!.FamilyId!.Value, person!.Family!.Name!)
            : null;

        return Task.FromResult(resolvedReference);
    }

    public IList<string> ParsePhoneNumbers(IList<string> references)
    {
        return references
            .Where(x => IsValidReference(x))
            .Select(Parse)
            .Select(x => x.PhoneNumber)
            .Where(x => !x.IsNullOrEmpty())
            .ToList();
    }

    public static (string ChurchCode, string? PhoneNumber, GivingType Type, string? PartnershipFund, bool IsFamily) Parse(string reference)
    {
        reference = reference.Trim().ToUpperInvariant();

        var parts = reference.Split('-');
        var churchShortCode = parts[0];
        var phoneNumber = parts[1].CleanPhoneNumber();
        var givingType = GivingType.FromInitials(parts[2]);
        var isFamily = reference.EndsWith("F");

        string? partnershipFund = null;
        if (givingType == GivingType.Partnership)
        {
            partnershipFund = parts[3];
        }

        return (churchShortCode, phoneNumber, givingType, partnershipFund, isFamily);
    }

    public static bool IsValidReference(string? reference) => !reference.IsNullOrEmpty() && Regex.IsMatch(reference!, ReferencePattern);

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
}