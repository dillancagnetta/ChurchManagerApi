using Codeboss.Results;

namespace ChurchManager.Domain.Features.Communications.Services;


public interface IEmailOrchestrator
{
    Task<OperationResult<string>> SendEmailAsync(
        EmailRecipient recipient,
        string subject,
        TemplateInfo templateInfo,
        CancellationToken ct = default);
    
    Task<OperationResult<string>> SendEmailAsync(
        EmailRecipient recipient,
        string subject,
        string body,
        CancellationToken ct = default);
}