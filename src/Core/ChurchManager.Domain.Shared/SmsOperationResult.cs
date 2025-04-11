namespace ChurchManager.Domain.Shared;

public record SmsOperationResult
{
    public int PersonId { get; set; }
    public string MessageId { get; set; }
    public bool IsSent { get; set; }
    public string Error { get; set; }
};