using ChurchManager.Infrastructure.Abstractions;

namespace ChurchManager.Domain.Features.ChangeRequests.Events;

public record PersonBaptismChangeRequestApproved(int ChangeRequestId) : IDomainEvent;
public record ChangeRequestApprovedEvent(int ChangeRequestId) : IDomainEvent;
public record PersonBaptismDateChangeRequestApproved(int ChangeRequestId) : IDomainEvent;
public record PersonHolySpiritChangeRequestApproved(int ChangeRequestId) : IDomainEvent;