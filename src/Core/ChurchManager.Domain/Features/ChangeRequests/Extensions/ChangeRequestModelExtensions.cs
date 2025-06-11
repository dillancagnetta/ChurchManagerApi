using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.ChangeRequests.Extensions;

public static class ChangeRequestModelExtensions
{
    public static ChangeRequestViewModel? ToViewModel(this ChangeRequest? model, bool includeDetails = false)
    {
        if(model is null) return null;
        
        return new ChangeRequestViewModel
        {
            Id = model.Id,
            //PersonEntity = includeDetails ? model.Properties.FirstOrDefault()?..ToBasicPersonViewModel(),
            EntityType = includeDetails ? model.Properties.FirstOrDefault()?.EntityType : "",
            Properties = includeDetails ? model.Properties.Select(c => c.ToViewModel()).ToList() : [],
            RequestedDate = model.RequestedDate,
            ReviewNotes = model.ReviewNotes,
            Reason = model.Reason,
            Source = model.Source,
            Status = model.Status,
            ReviewedDate = model.ReviewedDate,
        };
    }
    
    public static PropertyChangeRequestViewModel ToViewModel(this PropertyChangeRequest model)
    {
        return new PropertyChangeRequestViewModel
        {
            Id = model.Id,
            PropertyPath = model.PropertyPath,
            EntityId = model.EntityId,
            CurrentValue = model.CurrentValue,
            RequestedValue = model.RequestedValue,
            //PropertyType = model.PropertyType,
            IsApplied = model.IsApplied
        };
    }
}