using ChurchManager.Domain.Common;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Domain.Features.Communication;

public class CommunicationContent : Entity<int>
{
    public CommunicationType CommunicationType { get; set; }

    public int CommunicationId { get; set; }

    // Stored as JSON for flexibility
    public string Content { get; set; }
    
    public string Metadata { get; set; }
    
    # region Navigation
    
    public virtual Communication Communication { get; set; }
    
    # endregion

}