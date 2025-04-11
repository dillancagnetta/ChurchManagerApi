using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Services;
using ChurchManager.Domain.Shared;
using Codeboss.Results;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Infrastructure.Shared.SMS;

public class BulkSmsSender : ISmsSender
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<BulkSmsSender> _logger;

    public BulkSmsSender(IHttpClientFactory httpClientFactory, ILogger<BulkSmsSender> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }
    
    public async Task<OperationResult<IEnumerable<SmsOperationResult>>> SendSmsAsync(BulkSmsMessage message, CancellationToken ct = default)
    {
        try
        {
            // Get the pre-configured HttpClient from the factory
            var httpClient = _httpClientFactory.CreateClient("BulkSmsClient");
                
            var request = new BulkSmsRequest
            {
                Body = message.Body,
                To = message.To.Select(x => x.PhoneNumber).ToList()
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"?deduplication-id={message.DeduplicationId}", content, ct);
                
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("SMS sent successfully to {RecipientCount} recipients", message.To.Count);
                var bulkSmsResponse = await response.Content.ReadFromJsonAsync<IList<BulkSmsResponse>>(cancellationToken: ct);
                
                // Skip the "+" as BulkSMS does not include that in the response
                var recipientMap = message.To.ToDictionary(
                    recipient => recipient.PhoneNumber[1..], // removes the first character
                    recipient => recipient.PersonId
                );               
                var smsOperationResults = bulkSmsResponse.Select(r => new SmsOperationResult
                {
                    PersonId = recipientMap[r.To],
                    MessageId = r.MessageId,
                    IsSent = r.Status.Type is not ("FAILED" or "UNKNOWN"),
                    Error = r.Status.Type is "FAILED" ? DeliveryStatusMessage(r.Status.SubType) : null
                });
                
                return new OperationResult<IEnumerable<SmsOperationResult>>(true, smsOperationResults);
            }

            // Error
            var error = await response.Content.ReadFromJsonAsync<BulkSmsErrorResponse>(ct);
            _logger.LogError("Failed to send SMS. Status: {StatusCode}, Error: {Error}", 
                response.StatusCode, error?.Detail);
            return OperationResult<IEnumerable<SmsOperationResult>>.Fail($"Failed to send sms: {error?.Detail}. Status code: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending SMS");
            return OperationResult<IEnumerable<SmsOperationResult>>.Fail(ex.Message);
        }
    }

    public async Task<OperationResult<SmsOperationResult>> SendSmsAsync(SmsMessage message, CancellationToken ct = default)
    {
        var bulkSmsMessage = new BulkSmsMessage
        {
            Body = message.Body,
            To =
            [
                new SmsRecipient { PersonId = message.Recipient.PersonId, PhoneNumber = message.Recipient.PhoneNumber }
            ],
        };

        var result = await SendSmsAsync(bulkSmsMessage, ct);

        return new OperationResult<SmsOperationResult>(result.IsSuccess, result.Result.First());
    }
    
    private string DeliveryStatusMessage(string status)
    {
        return status switch
        {
            "EXPIRED" => "Delivery failed because message expired before delivery was possible.",
            "HANDSET_ERROR" => "Delivery failed because of a problem related to the phone (e.g. message storage area full).",
            "BLOCKED" => "Your account has been blocked from sending to this phone (e.g. recipient replied STOP to block communication).",
            "NOT_SENT" => "Message delivery was not attempted (e.g. because we were not able to find a route for the supplied phone number).",
            _ => "Unknown status."
        };
    }
}