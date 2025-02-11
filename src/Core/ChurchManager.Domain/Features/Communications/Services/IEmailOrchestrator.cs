using Codeboss.Results;

namespace ChurchManager.Domain.Features.Communications.Services;


public interface IEmailOrchestrator
{
    Task<OperationResult> SendEmailAsync(
        EmailRecipient recipient,
        string subject,
        TemplateInfo templateInfo,
        CancellationToken ct = default);
}