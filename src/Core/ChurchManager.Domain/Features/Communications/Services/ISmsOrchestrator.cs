using ChurchManager.Domain.Shared;
using Codeboss.Results;

namespace ChurchManager.Domain.Features.Communications.Services;

public interface ISmsOrchestrator
{
    Task<OperationResult<IEnumerable<SmsOperationResult>>> SendBulkSmsAsync(
        BulkSmsMessage message,
        CancellationToken ct = default);
    
    Task<OperationResult<SmsOperationResult>> SendSmsAsync(
        SmsMessage message,
        TemplateInfo templateInfo,
        CancellationToken ct = default);
    
    IAsyncEnumerable<OperationResult<SmsOperationResult>> SendSmsAsync(
        IEnumerable<SmsMessage> messages,
        TemplateInfo templateInfo,
        CancellationToken ct = default);
}