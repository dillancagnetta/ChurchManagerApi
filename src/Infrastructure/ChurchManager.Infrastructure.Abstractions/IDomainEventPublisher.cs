using ChurchManager.Domain.Shared;

namespace ChurchManager.Infrastructure.Abstractions;

public interface IDomainEventPublisher
{
    ValueTask PublishAsync(IDomainEvent @event, CancellationToken ct = default);
}

/// <summary>
///     Marker interface to denote that this class should be considered
///     as a domain message handler regardless of naming convention
/// </summary>
public interface IDomainEventHandler;