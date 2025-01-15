using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Communication;
using Codeboss.Types;

namespace ChurchManager.Domain.Features;

/// <summary>
/// Represents a message entity for persistent messages
/// </summary>
public class Message : IAggregateRoot<int>, IHaveUserId<Guid>
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(100)]
    public string Title { get;  set; }
    
    [Required]
    public string Body { get; set; }
    
    public DateTime? SentDateTime { get; set; }
    
    [MaxLength( 100 )]
    public string? IconCssClass { get;  set; }
    
    [MaxLength( 200 )]

    public string? ImagePath { get;  set; }
    
    [Required, MaxLength(30)]
    public MessageClassification Classification { get;  set; }
    
    [MaxLength( 300 )]
    public string? Link { get;  set; }
    
    public bool UseRouter { get;  set; }
    public bool IsRead { get;  set; }

    public bool SendWebPush { get; set; } = true;
    
    [Required]
    public Guid UserId { get; set; } 
    
    // Sent properties
    public MessageStatus Status { get;  set; }
    
    [MaxLength( 200 )]
    public string? LastError { get;   set; }

    public Message()
    {
        Status = MessageStatus.Pending;
    }

    private Message(MessageClassification classification, string title, string body, Guid userId)
    {
        Status = MessageStatus.Pending;
        Classification = classification;
        Title = title;
        Body = body;
        UserId = userId;
        SendWebPush = true;
    }

    public static Message CreateInfoMessage(string title, string body, Guid userId) => new (MessageClassification.Info, title, body, userId);
    public static Message CreateWarningMessage(string title, string body, Guid userId) => new (MessageClassification.Warning, title, body, userId);
    public static Message CreateSuccessMessage(string title, string body, Guid userId) => new (MessageClassification.Success, title, body, userId);
    
    #region Methods

    public void MarkAsSent(IDateTimeProvider? dateTime)
    {
        Status = MessageStatus.Sent;
        SentDateTime = dateTime?.Now ?? DateTime.UtcNow;
    }

    public void MarkAsFailed(string error)
    {
        Status = MessageStatus.Failed;
        LastError = error;
    }
    
    public void MarkAsFailed(Exception error)
    {
        Status = MessageStatus.Failed;
        LastError = error.Message;
    }

    #endregion
    
    #region Navigation

    public virtual UserLogin UserLogin { get; set; }

    #endregion
}