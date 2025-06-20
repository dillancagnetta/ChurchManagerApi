using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features.Finances;
using ChurchManager.Domain.Features.Finances.Services;
using Codeboss.Types;

namespace ChurchManager.Features.Finances.Services;

public class PaymentReferenceGenerator : IPaymentReferenceGenerator
{
    /// <summary>
    ///Example formats:
    ///  Person: GIFT-P-1-T
    ///  Family: GIFT-F-2-O
    ///  Group:  GIFT-G-3-FF
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="givingType"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public string GeneratePaymentReference<T>(T entity, GivingType givingType) where T : IEntity<int>
    {
        var idPart = entity.Id;
        var entityType = typeof(T).Name.Substring(0, 1);
        var typeCode  = givingType.ToString().ToInitials();
        return $"GIFT-{entityType}-{idPart}-{givingType}";
    }

    public (int? EntityId, char? EntityType, GivingType? givingType) ParseReference(string reference)
    {
        if (string.IsNullOrEmpty(reference) || !reference.StartsWith("GIFT-"))
            return (null, null, null);
            
        var parts = reference.Split('-');
        if (parts.Length != 4) return (null, null, null);
            
        try
        {
            // e.g Person: GIFT-P-1-T
            var entityType = parts[1][0]; // P
            var idPart = parts[2]; // "1"
            var typeCode = parts[3]; // "T"
            
            var entityId = int.Parse(idPart);
            var givingType = GivingType.FromInitials(typeCode);
            
            return (entityId, entityType, givingType);
        }
        catch
        {
            return (null, null, null);
        }
    }
}