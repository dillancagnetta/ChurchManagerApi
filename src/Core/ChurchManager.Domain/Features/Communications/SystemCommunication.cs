using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Domain.Features.Communications;

/// <summary>
/// Represents a System email template.
/// </summary>
/// 
[Table("SystemCommunication", Schema = "Communications")]
public class SystemCommunication : Entity<int>
{
    
}