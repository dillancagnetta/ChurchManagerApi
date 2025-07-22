using System.Text.RegularExpressions;
using ChurchManager.Domain.Common.Extensions;
using CodeBoss.Extensions;

namespace ChurchManager.Domain.Features.Finances.Extensions;

public static class FinancesExtensions
{
    /// <summary>
    /// Format: 3 letters - 10-digit number starting with 0 - at least one letter
    /// If the first letter after the number is P, there must be another letter segment (e.g., -P-HS)
    /// Final optional -F
    /// Example: CHU-0821234567-T
    /// </summary>
    const string ReferencePattern = @"^[A-Za-z]{3}-0\d{9}-(?!P$)(?:[A-Za-z]+(?:-[A-Za-z]+)*)(?:-F)?$";
    
    public static (string ChurchCode, string? PhoneNumber, GivingType Type, string? PartnershipFund, bool IsFamily) ParsePaymentReference(this string reference)
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
    
    public static bool IsValidPaymentReference(this string? reference) => !reference.IsNullOrEmpty() && Regex.IsMatch(reference!, ReferencePattern);
}