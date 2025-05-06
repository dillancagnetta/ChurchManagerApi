using ChurchManager.Infrastructure.Abstractions;

namespace ChurchManager.Domain.Features.Communications.Events;

public record MessageForUserAddedEvent(int MessageId) : IDomainEvent;