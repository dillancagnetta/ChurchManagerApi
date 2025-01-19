using Codeboss.Types;

namespace ChurchManager.Domain.Features.Communication;

public class MessageClassification: Enumeration<MessageClassification, string>
{
    public MessageClassification(string value) => Value = value;
    
    public static MessageClassification Success = new("Success");
    public static MessageClassification Info = new("Info");
    public static MessageClassification Warning = new("Warning");
    public static MessageClassification Danger = new("Danger");
    public static MessageClassification System = new("System");
    // Implicit conversion from string
    public static implicit operator MessageClassification(string value) => new(value);
}


public class MessageStatus: Enumeration<MessageStatus, string>
{
    public MessageStatus(string value) => Value = value;
    
    public static MessageStatus Pending = new("Pending");
    public static MessageStatus Sent = new("Sent");
    public static MessageStatus Failed = new("Failed");
    // Implicit conversion from string
    public static implicit operator MessageStatus(string value) => new(value);
}