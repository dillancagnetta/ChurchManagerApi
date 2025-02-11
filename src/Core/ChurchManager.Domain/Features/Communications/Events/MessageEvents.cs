using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Communications.Events;

public record MessageForUserAddedEvent(int MessageId) : IDomainEvent;