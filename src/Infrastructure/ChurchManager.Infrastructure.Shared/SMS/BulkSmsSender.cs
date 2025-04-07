using System.Text;
using System.Text.Json;
using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Services;
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
    
    public async Task<OperationResult> SendSmsAsync(SmsMessage message)
    {
        try
        {
            // Get the pre-configured HttpClient from the factory
            var httpClient = _httpClientFactory.CreateClient("BulkSmsClient");
                
            var request = new BulkSmsRequest
            {
                Body = message.Body,
                To = message.To.ToList()
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("", content);
                
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("SMS sent successfully to {RecipientCount} recipients", message.To.Count);
                return OperationResult.Success();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to send SMS. Status: {StatusCode}, Error: {Error}", 
                    response.StatusCode, error);
                return OperationResult.Fail(error);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending SMS");
            return OperationResult.Fail(ex.Message);
        }
    }
}