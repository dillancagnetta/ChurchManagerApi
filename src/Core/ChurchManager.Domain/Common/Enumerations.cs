using Codeboss.Types;

namespace ChurchManager.Domain.Common
{
    public class RecordStatus : Enumeration<RecordStatus, string>
    {
        public RecordStatus(string value) => Value = value;

        public static RecordStatus Active = new("Active");
        public static RecordStatus InActive = new("InActive");
        public static RecordStatus Pending = new("Pending");
        // Implicit conversion from string
        public static implicit operator RecordStatus(string value) => new(value);
        // Implicit conversion to bool
        public static implicit operator bool(RecordStatus status) =>
            status != null && !string.IsNullOrEmpty(status.Value) && status.Value == Active;
    }

    /// <summary>
    /// Represents the communication preference of a <see cref="CommunicationType"/> in a <see cref="Person"/>.
    /// </summary>
    public class CommunicationType : Enumeration<CommunicationType, string>
    {
        public CommunicationType(string value) => Value = value;

        public static CommunicationType WhatsApp = new("WhatsApp");
        public static CommunicationType Signal = new("Signal");
        public static CommunicationType Email = new("Email");
        public static CommunicationType SMS = new("SMS");
        public static CommunicationType None = new("None");

        public static implicit operator CommunicationType(string value) => new(value);
    }

    public class HistoryVerb : Enumeration<HistoryVerb, string>
    {
        public HistoryVerb(string value) => Value = value;

        public static HistoryVerb Add = new("Add");
        public static HistoryVerb Modify = new("Modify");
        public static HistoryVerb Delete = new("Delete");
        public static HistoryVerb Registered = new("Registered");
        public static HistoryVerb Process = new("Process");
        public static HistoryVerb Matched = new("Matched");
        public static HistoryVerb Unmatched = new("Unmatched");
        public static HistoryVerb Login = new("Login");
        public static HistoryVerb Merge = new("Merge");
        public static HistoryVerb AddedToGroup = new("AddedToGroup");
        public static HistoryVerb RemovedFromGroup = new("RemovedFromGroup");
        public static HistoryVerb StepAdded = new("StepAdded");

        public static implicit operator HistoryVerb(string value) => new(value);
    }

    public class ApprovalStatus : Enumeration<ApprovalStatus, string>
    {
        public ApprovalStatus(string value) => Value = value;

        /// <summary>
        ///  has been submitted but not yet approved or denied
        /// </summary>
        public static ApprovalStatus PendingApproval = new("PendingApproval");
    
        /// <summary>
        ///  has been approved 
        /// </summary>
        public static ApprovalStatus Approved = new("Approved");
    
        /// <summary>
        ///  has been denied
        /// </summary>
        public static ApprovalStatus Denied = new ApprovalStatus("Denied");
        
        public static implicit operator ApprovalStatus(string value) => new(value);

    }
}
