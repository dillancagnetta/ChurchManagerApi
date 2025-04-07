namespace ChurchManager.Domain.Features.Communications;

public class SmsMessage
{
    public string Body { get; set; }
    public ICollection<string> To { get; set; } = new List<string>();
    public string From { get; set; }
}