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
    IQueryCache cache
) : IGivingReferenceResolver
{
    /// <summary>
    ///Example formats:
    ///  Person: CHU-082xxxxxxx-T
    ///  Family: CHU-082xxxxxxx-P-HS-F
    /// </summary>
    public async Task<BankStatementImport> ResolveAsync(BankStatementImport import)
    {
        var peoplePhoneMap =
            await peopleDb.FindPhoneNumberForPeople(ParsePhoneNumbers(import.OriginalTransactionReferences()));

        foreach (var transaction in import.Transactions)
        {
            var reference = transaction.OriginalReference;
            var parsed = Parse(reference);

            var resolvedReference = new GivingReference
            {
                GivingType = parsed.Type,
                BenefactorType = parsed.IsFamily ? BenefactorType.Family : BenefactorType.Individual,
                IsParsed = parsed.Type != GivingType.Unknown
            };

            resolvedReference = await ResolveChurchAsync(reference, resolvedReference);
            resolvedReference = await ResolvePersonAsync(reference, resolvedReference, peoplePhoneMap);
            
            transaction.IsMatched = resolvedReference.IsParsed;

            if (transaction.IsMatched)
            {
                transaction.Giving = Giving.Create(transaction, resolvedReference, null, null, transaction.Memo);
            }
        }

        return import;
    }

    public async Task<GivingReference> ResolveChurchAsync(string reference, GivingReference resolvedReference)
    {
        var (churchCode, _, _, _) = Parse(reference);

        var churches = await cache.GetOrSetAsync("churches", () => churchesDb.Queryable().AsNoTracking().ToListAsync());
        var church = churches.FirstOrDefault(x => x.ShortCode == churchCode);

        resolvedReference.ChurchId = church?.Id;
        resolvedReference.IsParsed = resolvedReference.IsParsed && church is not null;

        return resolvedReference;
    }

    public Task<GivingReference> ResolvePersonAsync(string reference, GivingReference resolvedReference,
        Dictionary<string, Person?> map)
    {
        var (_,phoneNumber, _, isFamily) = Parse(reference);
        var person = map.GetValueOrDefault(phoneNumber);

        resolvedReference.PersonId = person?.Id;
        resolvedReference.FamilyId = isFamily ? person?.FamilyId : null;
        resolvedReference.IsParsed = resolvedReference.IsParsed && person is not null;

        return Task.FromResult(resolvedReference);
    }

    public IList<string> ParsePhoneNumbers(IList<string> references)
    {
        return references.Select(Parse)
            .Select(x => x.PhoneNumber)
            .Where(x => !x.IsNullOrEmpty())
            .ToList();
        ;
    }

    public (string ChurchCode, string PhoneNumber, GivingType Type, bool IsFamily) Parse(string reference)
    {
        reference = reference.Trim().ToUpperInvariant();

        var parts = reference.Split('-');
        var churchShortCode = parts[0];
        var phoneNumber = parts[1];
        var givingType = GivingType.FromInitials(parts[2]);
        var isFamily = reference.EndsWith("F");

        return (churchShortCode, phoneNumber, givingType, isFamily);
    }
}