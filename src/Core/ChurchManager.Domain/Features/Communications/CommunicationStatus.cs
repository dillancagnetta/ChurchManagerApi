using Codeboss.Types;

namespace ChurchManager.Domain.Features.Communications;

public class CommunicationStatus : Enumeration<CommunicationStatus, string>
{
    public CommunicationStatus(string value) => Value = value;
    public CommunicationStatus() { Value = "Transient"; }

    /// <summary>
    /// Communication was created, but not yet edited by a user. (i.e. from data grid or report)
    /// Transient communications more than a few hours old may be deleted by clean-up job.
    /// </summary>
    public static CommunicationStatus Transient = new("Transient");
    
    /// <summary>
    /// Communication is currently being drafted
    /// </summary>
    public static CommunicationStatus Draft = new("Draft");
    
    /// <summary>
    /// Communication has been submitted but not yet approved or denied
    /// </summary>
    public static CommunicationStatus PendingApproval = new("PendingApproval");
    
    /// <summary>
    /// Communication has been approved for sending
    /// </summary>
    public static CommunicationStatus Approved = new("Approved");
    
    /// <summary>
    /// Communication has been denied
    /// </summary>
    public static CommunicationStatus Denied = new("Denied");
    // Implicit conversion from string
    public static implicit operator CommunicationStatus(string value) => new(value);
}

public class CommunicationRecipientStatus : Enumeration<CommunicationRecipientStatus, string>
{
    public CommunicationRecipientStatus(string value) => Value = value;
    
    public CommunicationRecipientStatus() => Value = "Unknown";

    /// <summary>
    /// Communication has not yet been sent to recipient
    /// </summary>
    public static CommunicationRecipientStatus Pending = new("Pending");
    
    /// <summary>
    /// Communication was successfully delivered to recipient's mail server
    /// </summary>  
    public static CommunicationRecipientStatus Delivered = new("Delivered");
    
    public static CommunicationRecipientStatus Sent = new("Sent");
    
    
    public static CommunicationRecipientStatus Failed = new("Failed");
    
    /// <summary>
    /// Communication was cancelled prior to sending to the recipient
    /// </summary>
    public static CommunicationRecipientStatus Cancelled = new("Cancelled");
    
    
    public static CommunicationRecipientStatus Opened = new("Opened");
    
    /// <summary>
    /// Temporary status used while sending ( to prevent transaction and job sending same record )
    /// </summary>
    public static CommunicationRecipientStatus Sending = new("Sending");
    // Implicit conversion from string
    public static implicit operator CommunicationRecipientStatus(string value) => new(value);
}