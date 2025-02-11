using System.Text.Json;
using ChurchManager.Api.Middlewares;
using ChurchManager.Features.Communication.Commands;
using ChurchManager.SharedKernel.Common;
using ChurchManager.SharedKernel.Wrappers;
using CodeBoss.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class CommunicationsController(ILogger<CommunicationsController> logger) : BaseApiController
{
    
    /// <summary>
    /// Local test via ngrok
    /// ngrok http 5000
    /// e.g.  https://67d7-165-73-121-105.ngrok-free.app/api/v1/communications/bounce-emails-notification
    /// </summary>
    [HttpPost("bounce-emails-notification")]
    [AllowAnonymous]
    [ServiceFilter(typeof(AwsIpFilterAttribute))]
    [Consumes("text/plain", "text/plain; charset=utf-8", "application/json", "application/x-www-form-urlencoded")]
    public async Task<IActionResult> ReceiveNotification()
    {
        string rawMessage;
        using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
        {
            rawMessage = await reader.ReadToEndAsync();
        }
        
        logger.LogInformation($"Received raw message: {rawMessage}");
    
        // Parse the raw message
        var message = ParseSnsMessage(rawMessage);

        switch (message.Type)
        {
            case "SubscriptionConfirmation":
                await ConfirmSubscription(message.SubscribeURL);
                break;
            case "Notification":
                await ProcessNotification(message);
                break;
            default:
                logger.LogWarning($"Received unknown message type: {message.Type}");
                break;
        }

        return Ok();
    }
    
    private SnsMessage ParseSnsMessage(object rawMessage)
    {
        if (rawMessage is string stringMessage)
        {
            // Try to parse as JSON
            try
            {
                return JsonSerializer.Deserialize<SnsMessage>(stringMessage);
            }
            catch (JsonException)
            {
                // If it's not JSON, it might be URL-encoded form data
                var formData = System.Web.HttpUtility.ParseQueryString(stringMessage);
                return new SnsMessage
                {
                    Type = formData["Type"],
                    MessageId = formData["MessageId"],
                    TopicArn = formData["TopicArn"],
                    Message = formData["Message"],
                    Timestamp = formData["Timestamp"],
                    SignatureVersion = formData["SignatureVersion"],
                    Signature = formData["Signature"],
                    SigningCertURL = formData["SigningCertURL"],
                    UnsubscribeURL = formData["UnsubscribeURL"],
                    SubscribeURL = formData["SubscribeURL"],
                    Token = formData["Token"]
                };
            }
        }
        else if (rawMessage is JsonElement jsonElement)
        {
            return JsonSerializer.Deserialize<SnsMessage>(jsonElement.GetRawText());
        }
    
        return null;
    }
    
    private async Task ConfirmSubscription(string subscribeUrl)
    {
        using var httpClient = new HttpClient();
        await httpClient.GetAsync(subscribeUrl);
        logger.LogInformation($"Subscription confirmed: {subscribeUrl}");
    }
    
    private async Task ProcessNotification(SnsMessage message)
    {
        // Your existing code for handling SNS messages
        logger.LogInformation($"Processing SNS message: {message.Message}");
    }

    
    
   /*
    {
      "personIds": [
        1
      ],
      "communicationType": "Email",
      "subject": "Test email",
      "communicationTemplateId": 2,
      "senderPersonId": 1
    }
    */
    [HttpPost("create-communication")]
    public async Task<IActionResult> CreateCommunication([FromBody] CreateCommunicationCommand command, CancellationToken token)
    {
        return Ok(await Mediator.Send(command, token));
    }
    
    [HttpPost("approve-communication")]
    public async Task<IActionResult> ApproveCommunication([FromBody] ApproveCommunicationCommand command, CancellationToken token)
    {
        return Ok(await Mediator.Send(command, token));
    }
}

public record SnsMessage
{
    public string Type { get; set; }
    public string MessageId { get; set; }
    public string TopicArn { get; set; }
    public string Message { get; set; }
    public string Timestamp { get; set; }
    public string SignatureVersion { get; set; }
    public string Signature { get; set; }
    public string SigningCertURL { get; set; }
    public string UnsubscribeURL { get; set; }
    
    // Added for subscription confirmation
    public string SubscribeURL { get; set; }
    public string Token { get; set; }
}