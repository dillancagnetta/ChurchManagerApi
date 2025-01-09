using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Communication;
using Codeboss.Types;

namespace ChurchManager.Domain.Features;

/// <summary>
/// Represents a message entity for persistent messages
/// </summary>
public class Message : IAggregateRoot<int>
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(100)]
    public string Title { get; set; }
    
    [Required]
    public string Body { get; set; }
    
    [Required]
    public DateTime SentDateTime { get; set; }
    
    [MaxLength( 100 )]
    public string? IconCssClass { get; set; }
    
    [Required, MaxLength(30)]
    public MessageClassification Classification { get; set; }
    
    [MaxLength( 300 )]
    public string? Link { get; set; }
    
    public bool UseRouter { get; set; }
    
    public bool IsRead { get; set; }
    
    public Guid UserLoginId { get; set; } 
    
    #region Navigation

    public virtual UserLogin UserLogin { get; set; }

    #endregion
}