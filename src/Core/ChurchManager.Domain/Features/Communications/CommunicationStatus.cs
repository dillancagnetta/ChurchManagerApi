using Codeboss.Types;

namespace ChurchManager.Domain.Features.Communication;

public class CommunicationStatus : Enumeration<CommunicationStatus, string>
{
    public CommunicationStatus(string value) => Value = value;

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