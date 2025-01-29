using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Events.DomainEvents;

// Group = EventRegistrationGroup
public record PersonRegisteredForEvent(int PersonId, int GroupId, int GroupRoleId): IDomainEvent;