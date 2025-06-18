using ChurchManager.Infrastructure.Abstractions;
using Wolverine;

namespace ChurchManager.Infrastructure.Shared.DomainEvents
{
    public class WolverineRabbitMqDomainEventPublisher(IMessageBus bus) : IDomainEventPublisher
    {

        public ValueTask PublishAsync(IDomainEvent @event, CancellationToken ct = default)
        {
            return bus.PublishAsync(@event);
        }
    }
}
