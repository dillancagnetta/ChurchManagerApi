using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Features.Communications.Events;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Domain.Features.Communications;

public class Communication : AuditableEntity<int>, IAggregateRoot<int>
{
    [MaxLength( 100 )]
    public string Name { get;  set; }
    
    /// <summary>
    /// Gets or sets the subject or title of the communication.
    /// </summary>
    [MaxLength( 100 )]
    public string? Subject { get; set; }
    
    [MaxLength( 50 )]
    public CommunicationType CommunicationType  { get;  set; }
    
    /// <summary>
    /// Gets or sets the <see cref="Group">list</see> that email is being sent to.
    /// </summary>
    public int? ListGroupId { get;  set; }
    
    /// <summary>
    /// Gets or sets the <see cref="Communications.CommunicationTemplate"/> that was used to compose this communication
    /// </summary>
    public int? CommunicationTemplateId { get;  set; }
    
    /// <summary>
    /// Gets or sets the content if no template was used.
    /// </summary>
    public string CommunicationContent { get; set; }

    /// <summary>
    /// Gets or sets the sender <see cref="Person"/> identifier.
    /// </summary>
    public int? SenderPersonId { get;   set; }
    
    /// <summary>
    /// Gets or sets the is bulk communication.
    /// </summary>
    public bool IsBulkCommunication { get;  set; }
    
    /// <summary>
    /// Gets or sets the datetime that communication was sent. This also indicates that communication shouldn't attempt to send again.
    /// </summary>
    public DateTime? SendDateTime { get;  set; }
    
    /// <summary>
    /// Gets or sets the future send date for the communication. This allows a user to schedule when a communication is sent 
    /// and the communication will not be sent until that date and time.
    /// </summary>
    public DateTime? FutureSendDateTime { get;  set; }
    
    [MaxLength( 100 )]
    public CommunicationStatus Status { get;  set; }
    
    public CommunicationReview Review { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the message meta data.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; }
    
    public int? SystemCommunicationId { get; set; }

    
    # region Navigation Properties
    
    public virtual ICollection<CommunicationRecipient> Recipients { get; set; }
    public virtual ICollection<CommunicationAttachment> Attachments { get; set; }
    public virtual CommunicationTemplate CommunicationTemplate { get; set; }
    public virtual Group ListGroup { get; set; }
    public virtual SystemCommunication SystemCommunication { get; set; }
    public virtual Person SenderPerson { get; set; }
    
    # endregion

    #region Methods

    public void Approve(CommunicationReview review)
    {
        Review = review;
        Status = CommunicationStatus.Approved;
        
        AddDomainEvent(new CommunicationApprovedEvent(Id));
    }
    
    public bool IsTemplatedCommunication() => CommunicationTemplateId.HasValue;

    #endregion
}

[Owned]
public record CommunicationReview
{
    public string ReviewerNote { get; set; }
    public DateTime? ReviewedDateTime { get; set; }
    public int? ReviewerPersonId { get; set; }
}