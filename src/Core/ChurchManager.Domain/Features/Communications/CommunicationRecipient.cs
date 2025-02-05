using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Domain.Features.Communication;

public class CommunicationRecipient : Entity<int>
{
    /// <summary>
    /// Gets or sets the CommunicationId of the <see cref="Communication"/>.
    /// </summary>
    public int CommunicationId { get; set; }
    
    /// <summary>
    /// Gets or sets the PersonId of the <see cref="Person"/> who is being sent the <see cref="Rock.Model.Communication"/>.
    /// </summary>
    public int PersonId { get; set; }

    /// <summary>
    /// Gets or sets the status of the Communication submission to the recipient.
    /// </summary>
    public CommunicationRecipientStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the status note.
    /// </summary>
    public string StatusNote { get; set; }
    
    /// <summary>
    /// Gets or sets the datetime that communication was sent.
    /// </summary>
    public DateTime? SendDateTime { get; set; }

    /// <summary>
    /// Gets or sets the datetime that communication was opened by recipient.
    /// </summary>
    public DateTime? OpenedDateTime { get; set; }
    
    /// <summary>
    /// Gets or sets the unique message identifier.
    /// </summary>
    [MaxLength( 100 )]
    public string UniqueMessageId { get; set; }
    
    # region Navigation
    
    public virtual Communication Communication { get; set; }
    
    # endregion
}