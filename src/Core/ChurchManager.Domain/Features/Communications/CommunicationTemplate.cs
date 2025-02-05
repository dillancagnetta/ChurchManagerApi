using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Common;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Communications;

public class CommunicationTemplate: AuditableEntity<int>, IAggregateRoot<int>
{
    /// <summary>
    /// Gets or sets the name of the Communication Template
    /// </summary>
    /// <value>
    /// A <see cref="System.String"/> that represents the name of the communication template
    /// </value>
    [Required, MaxLength( 100 )]
    public string Name { get; set; }
    
    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Gets or sets a flag indicating if this PageContext is a part of the core system/framework. This property is required.
    /// </summary>
    [Required]
    public bool IsSystem { get; set; }

    [MaxLength(25)]
    public string RecordStatus { get; set; } = "Active";
    
    /// <summary>
    /// Gets or sets the logo url that email messages using this template can use for the logo in the message content
    /// </summary>
    /// <value>
    /// The logo url.
    /// </value>
    public string LogoFileUrl { get; set; }
    
    [MaxLength(25)]
    public string Category { get; set; }
    
    /// <summary>
    /// Gets or sets the template content.
    /// </summary>
    public string Content { get; set; }

    public ICollection<CommunicationType> SupportedTypes { get; set; }

    /// <summary>
    /// Gets or sets whether the template is a base or layout template.
    /// </summary>
    public bool IsBaseTemplate { get; set; }
}