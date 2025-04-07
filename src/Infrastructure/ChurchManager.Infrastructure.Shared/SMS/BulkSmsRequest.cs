namespace ChurchManager.Infrastructure.Shared.SMS;

using System.Text.Json.Serialization;

public class BulkSmsRequest
{
    [JsonPropertyName("body")]
    public string Body { get; set; }

    [JsonPropertyName("to")]
    public List<string> To { get; set; } = new List<string>();
}