using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Services;
using Codeboss.Results;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Infrastructure.Shared.SMS;

public class FakeSmsSender : ISmsSender
{
    private readonly Random _random = new Random();

    public Task<OperationResult<IEnumerable<SmsOperationResult>>> SendSmsAsync(BulkSmsMessage message, CancellationToken ct = default)
    {
        var results = new List<SmsOperationResult>();

        foreach (var recipient in message.To)
        {
            var smsResult = new SmsOperationResult
            {
                PersonId = recipient.PersonId,
            };

            // 80% success rate for each individual message
            if (_random.NextDouble() < 0.8)
            {
                // Generate a unique GUID for successful sends
                string messageId = Guid.NewGuid().ToString();
                smsResult.IsSent = true;
                smsResult.MessageId = messageId;
            }
            else
            {
                // Simulate sending failure
                smsResult.Error = "Failed to send SMS";
                smsResult.IsSent = false;
            }

            results.Add(smsResult);
        }

        // Simulate some processing time
        Task.Delay(_random.Next(1, 10) * 1000, ct).Wait();

        return Task.FromResult(OperationResult<IEnumerable<SmsOperationResult>>.Success(results));
    }

    public Task<OperationResult<SmsOperationResult>> SendSmsAsync(SmsMessage message, CancellationToken ct = default)
    {
        // Simulate some processing time
        Task.Delay(_random.Next(1, 10) * 1000, ct).Wait(); 
        
        var smsResult = new SmsOperationResult
        {
            PersonId = message.Recipient.PersonId,
        };
        // 80% success rate
        if (_random.NextDouble() < 0.8)
        {
            // Generate a unique GUID for successful sends
            string messageId = Guid.NewGuid().ToString();
            smsResult.IsSent = true;
            smsResult.MessageId = messageId;
            return Task.FromResult(OperationResult<SmsOperationResult>.Success(smsResult));
        }
        
        // Simulate sending failure
        smsResult.Error = "NOT_SENT";
        smsResult.IsSent = false;
        return Task.FromResult(new OperationResult<SmsOperationResult>(true, smsResult));
    }
}