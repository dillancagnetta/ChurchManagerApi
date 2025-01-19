using ChurchManager.Persistence.Shared;

namespace ChurchManager.Domain.Features.Communication;

public class CommunicationAttachment: Entity<int>
{
    public int CommunicationId { get; set; }

    
    # region Navigation
    
    public virtual Communication Communication { get; set; }
    
    # endregion
}