using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Finances.Services;

public interface IPaymentReferenceGenerator
{
    string GeneratePaymentReference<T>(T entity, GivingType givingType) where T: IEntity<int>;

    (int? EntityId, char? EntityType, GivingType? givingType) ParseReference(string reference);
}