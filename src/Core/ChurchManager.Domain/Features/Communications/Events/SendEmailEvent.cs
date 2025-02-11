using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Communications.Events
{
    public record SendEmailEvent(string Subject, string Template, EmailRecipient Recipient) : IDomainEvent
    {
        public IDictionary<string, string> TemplateData { get; set; }
    }
}
