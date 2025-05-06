using ChurchManager.Domain.Features.People.Extensions;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Churches.Extensions;

public static class ChurchModelExtensions
{
    
    public static ChurchGroupViewModel ToViewModel(this ChurchGroup model, bool includeDetails = false)
    {
        return new ChurchGroupViewModel
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            LeaderPerson = model.LeaderPerson.ToBasicPersonViewModel(),
            Churches = model.Churches.Select(c => c.ToViewModel(includeDetails)).ToList()
        };
    }
    
    public static ChurchViewModel ToViewModel(this Church model, bool includeDetails = false)
    {
        return new ChurchViewModel
        {
            Id = model.Id,
            Name = model.Name,
            ShortCode = model.ShortCode,
            PhoneNumber = model.ShortCode,
            Address = model.ShortCode,
            LeaderPerson = model.LeaderPerson.ToBasicPersonViewModel(),
            ChurchGroupId = model.ChurchGroupId,
            ServiceTimes = model.ServiceTimes.Select(st => st.ToViewModel()).ToList()
        };
    }
    
    public static ChurchServiceTimeViewModel ToViewModel(this ChurchServiceTime model)
    {
        return new ChurchServiceTimeViewModel
        {
            Id = model.Id,
            ChurchAttendanceType = model.ChurchAttendanceType?.Name,
            ChurchAttendanceTypeId = model.ChurchAttendanceTypeId,
            DayOfWeek = model.DayOfWeek,
            Time = model.Time
        };
    }
}