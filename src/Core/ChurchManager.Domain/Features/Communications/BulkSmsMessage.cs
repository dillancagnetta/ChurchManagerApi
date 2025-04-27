namespace ChurchManager.Domain.Features.Communications;

/// <summary>
/// Bulk SMS does not have customized body and is a general message to multiple recipients.
/// </summary>
public class BulkSmsMessage
{
    public required string Body { get; set; }
    public ICollection<SmsRecipient> To { get; set; } = new List<SmsRecipient>();
    public string? From { get; set; }
    
    /// <summary>
    /// Safeguards against the possibility of sending the same messages more than once.
    /// This will be the CommunicationId.
    /// </summary>
    public int DeduplicationId { get; set; }

    public FutureSendInfo? SendSchedule { get; set; }
}

public class SmsMessage
{
    public string? Body { get; set; }
    public SmsRecipient? Recipient { get; set; }
    public string? From { get; set; }
}

public record SmsRecipient
{
    public int PersonId { get; set; }
    public required string PhoneNumber { get; set; }
}

public record FutureSendInfo
{
    public DateTime ScheduleDate { get; set; }
    public string? ScheduleDescription { get; set; }
}