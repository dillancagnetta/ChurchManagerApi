using Codeboss.Results;

namespace ChurchManager.Domain.Features.Communications.Services
{
    public interface IEmailSender
    {
        Task<OperationResult> SendEmailAsync(EmailRecipient recipient, string subject, string htmlBody);
        /*Task<OperationResult> SendEmailAsync(Email email, string subject, string htmlBody);
        Task<OperationResult> SendEmailAsync(Person to, string subject, string htmlBody);*/
    }
}
