using System.ComponentModel.DataAnnotations;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Domain.Features.Communications;

public class CommunicationAttachment: AuditableEntity<int>
{
    public int CommunicationId { get; set; }

    public CommunicationType CommunicationType { get; set; }
    
    [Required]
    public bool IsSystem { get; set; }
    
    [Required, MaxLength( 255 )]
    public string FileName { get; set; }
    
    [Required, MaxLength( 255 )]
    public string MimeType { get; set; }
    
    public string Description { get; set; }
    
    /// <summary>
    /// File will have a URL or contents saved in the database
    /// </summary>
    [MaxLength( 255 )]
    public string FileUrl { get; set; }
    
    /// <summary>
    /// File will have a URL or contents saved in the database
    /// </summary>
    public string FileContents { get; set; }
    
    /// <summary>
    /// Gets or sets the size of the file (in bytes)
    /// </summary>
    public long? FileSize { get; set; }
    
    # region Navigation
    
    public virtual Communications.Communication Communication { get; set; }
    
    # endregion
}