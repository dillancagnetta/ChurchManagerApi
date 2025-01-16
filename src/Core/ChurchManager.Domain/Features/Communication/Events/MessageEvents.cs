using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Communication.Events;

public record MessageForUserAddedEvent(int MessageId) : IDomainEvent;