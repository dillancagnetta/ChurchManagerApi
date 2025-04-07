using Codeboss.Results;

namespace ChurchManager.Domain.Features.Communications.Services;

public interface ISmsSender
{
    Task<OperationResult> SendSmsAsync(SmsMessage message);
}