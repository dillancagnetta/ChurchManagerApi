using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Communications.Events;

public record CommunicationCreatedEvent(int CommunicationId) : IDomainEvent;
public record CommunicationApprovedEvent(int CommunicationId) : IDomainEvent;
public record CommunicationScheduledEvent(int CommunicationId) : IDomainEvent;
public record SendEmailToRecipientEvent(int CommunicationId, int RecipientId) : IDomainEvent;