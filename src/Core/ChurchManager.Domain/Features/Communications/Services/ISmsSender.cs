using ChurchManager.Domain.Shared;
using Codeboss.Results;

namespace ChurchManager.Domain.Features.Communications.Services;

public interface ISmsSender
{
    Task<OperationResult<IEnumerable<SmsOperationResult>>> SendSmsAsync(BulkSmsMessage message, CancellationToken ct = default);
    Task<OperationResult<SmsOperationResult>> SendSmsAsync(SmsMessage message, CancellationToken ct = default);
}