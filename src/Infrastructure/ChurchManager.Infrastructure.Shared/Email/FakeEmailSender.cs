using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Services;
using Codeboss.Results;

namespace ChurchManager.Infrastructure.Shared.Email;

public class FakeEmailSender : IEmailSender
{
    private readonly Random _random = new Random();

    public async Task<OperationResult<string>> SendEmailAsync(EmailRecipient recipient, string subject, string htmlBody)
    {
        await Task.Delay(_random.Next(1,10) * 1000);

        // 80% success rate
        if (_random.NextDouble() < 0.8)
        {
            // Generate a unique GUID for successful sends
            string messageId = Guid.NewGuid().ToString();
            return OperationResult<string>.Success(messageId);
        }

        // Simulate a failure
        return OperationResult<string>.Fail("Failed to send email");
    }
}