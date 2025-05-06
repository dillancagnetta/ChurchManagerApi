using ChurchManager.Infrastructure.Abstractions;
using Microsoft.Extensions.Logging;
using Wolverine;

namespace ChurchManager.Infrastructure.Shared.Tests
{
    public class TestDomainEventConsumer: IDomainEventHandler  
    {
        public ILogger<TestDomainEventConsumer> Logger { get; }

        public TestDomainEventConsumer(ILogger<TestDomainEventConsumer> logger)
        {
            Logger = logger;
        }

        public Task Handle(TestDomainEvent message, IMessageContext context)
        {
            Logger.LogInformation($"✔️ Message Received: {message.Content}");
            return Task.CompletedTask;
        }
    }

    public class TestDomainEvent : IDomainEvent
    {
        public string Content { get; set; } = "Hello";
    }
}
