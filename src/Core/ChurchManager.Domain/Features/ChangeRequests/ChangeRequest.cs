using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.ChangeRequests.Events;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.ChangeRequests;

/// <summary>
/// For handling external family updates to sensitive church records like baptism and Holy Spirit status
/// </summary>
[Table("ChangeRequest", Schema = "ChangeRequests")]
public class ChangeRequest : Entity<int>, IAggregateRoot<int>
{
    public DateTime RequestedDate { get; set; } = DateTime.UtcNow;
    public string? Reason { get; set; }
    [MaxLength(100)] public string? Source { get; set; }
    public ApprovalStatus Status { get; set; } = ApprovalStatus.PendingApproval.Value;
    
    /// <summary>
    /// Optional church this change request is for
    /// </summary>
    public int? ChurchId { get; set; }
    
    // Review
    public int? ReviewedByPersonId { get; set; }
    public DateTime? ReviewedDate { get; set; }
    public string? ReviewNotes { get; set; }
    
    // Navigation
    public virtual Person? ReviewedByPerson { get; set; }
    public virtual Church? Church { get; set; }
    
    public virtual ICollection<PropertyChangeRequest> Properties { get; set; } = new List<PropertyChangeRequest>();
    
    public void Approve(int reviewedByPersonId, string? notes)
    {
        ReviewedByPersonId = reviewedByPersonId;
        ReviewNotes = notes;
        ReviewedDate = DateTime.UtcNow;
        Status = ApprovalStatus.Approved;
        // Raise event
        AddDomainEvent(new ChangeRequestApprovedEvent(Id));
    }
}