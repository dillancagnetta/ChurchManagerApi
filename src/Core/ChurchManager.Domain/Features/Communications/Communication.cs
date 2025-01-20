using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Communication;

public class Communication : AuditableEntity<int>, IAggregateRoot<int>
{
    [MaxLength( 100 )]
    public string Name { get; private set; }
    
    [MaxLength( 50 )]
    public CommunicationType CommunicationType  { get; private set; }
    
    /// <summary>
    /// Gets or sets the <see cref="Group">list</see> that email is being sent to.
    /// </summary>
    /// <value>
    /// The list group identifier.
    /// </value>
    public int? ListGroupId { get; private set; }
    
    /// <summary>
    /// Gets or sets the <see cref="Rock.Model.CommunicationTemplate"/> that was used to compose this communication
    /// </summary>
    /// <value>
    /// The communication template identifier.
    /// </value>
    public int? CommunicationTemplateId { get; private  set; }

    /// <summary>
    /// Gets or sets the sender <see cref="Person"/> identifier.
    /// </summary>
    public int? SenderPersonId { get; private  set; }
    
    /// <summary>
    /// Gets or sets the is bulk communication.
    /// </summary>
    /// <value>
    /// The is bulk communication.
    /// </value>
    public bool IsBulkCommunication { get;private  set; }
    
    /// <summary>
    /// Gets or sets the datetime that communication was sent. This also indicates that communication shouldn't attempt to send again.
    /// </summary>
    /// <value>
    /// The send date time.
    /// </value>
    public DateTime? SendDateTime { get; private set; }
    
    /// <summary>
    /// Gets or sets the future send date for the communication. This allows a user to schedule when a communication is sent 
    /// and the communication will not be sent until that date and time.
    /// </summary>
    public DateTime? FutureSendDateTime { get; private set; }
    
    [MaxLength( 100 )]
    public CommunicationStatus Status { get; private set; }
    
    public Review Review { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the message meta data.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; }
    
    public int? SystemCommunicationId { get; set; }

    
    # region Navigation Properties
    
    public virtual ICollection<CommunicationRecipient> Recipients { get; set; }
    public virtual ICollection<CommunicationAttachment> Attachments { get; set; }
    public virtual CommunicationContent Content { get; set; }
    public virtual CommunicationTemplate CommunicationTemplate { get; set; }
    public virtual Group ListGroup { get; set; }
    public virtual SystemCommunication SystemCommunication { get; set; }
    public virtual Person SenderPerson { get; set; }
    
    # endregion
}