namespace ChurchManager.Infrastructure.Shared.SMS;

using System.Text.Json.Serialization;

public record BulkSmsRequest
{
    [JsonPropertyName("body")]
    public string Body { get; set; }

    [JsonPropertyName("to")]
    public List<string> To { get; set; } = [];
}


/*    Sample RESPONSE
 
 [ {
  "id" : "1491381497934909440",
  "type" : "SENT",
  "from" : "",
  "to" : "27737378631",
  "body" : "This is a test message from integration test.",
  "encoding" : "TEXT",
  "protocolId" : 0,
  "messageClass" : 0,
  "submission" : {
    "id" : "1-00000000002073112368",
    "date" : "2025-04-08T10:17:37Z"
  },
  "status" : {
    "id" : "ACCEPTED.null",
    "type" : "ACCEPTED",
    "subtype" : null
  },
  "relatedSentMessageId" : null,
  "userSuppliedId" : null,
  "numberOfParts" : null,
  "creditCost" : null
} ]
 */
public record BulkSmsResponse
{
    /// <summary>
    /// A unique identifier that is assigned when the message is created.
    /// </summary>
    [JsonPropertyName("id")]
    public string MessageId { get; set; }
    
    /// <summary>
    /// The message direction
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } // "SENT""RECEIVED"
    
    /// <summary>
    /// The phone number of the recipient
    /// </summary>
    [JsonPropertyName("to")]
    public string To { get; set; }
    
    [JsonPropertyName("status")]
    public BulkSmsResponseStatus Status { get; set; }
}

public record BulkSmsResponseStatus
{
    /// <summary>
    /// A concatenated value A.B where A is the status.type and B is the status.subtype.
    /// If there is no value for subtype then B takes string value "null" (e.g. "SENT.null").
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    /// <summary>
    /// "ACCEPTED""SCHEDULED""SENT""DELIVERED""UNKNOWN""FAILED"
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } 
    
    /// <summary>
    /// Has a value only if the type is FAILED
    /// "EXPIRED""HANDSET_ERROR""BLOCKED""NOT_SENT"
    /// </summary>
    [JsonPropertyName("subtype")]
    public string SubType { get; set; } 
}

public record BulkSmsErrorResponse
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
        
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("status")]
    public int Status { get; set; }
    
    [JsonPropertyName("detail")]
    public string Detail { get; set; }
}