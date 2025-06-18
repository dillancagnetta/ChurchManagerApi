using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Common.Extensions;

public static class CommonModelExtensions
{
    public static ScheduleViewModel? ToModel(this Schedule? model)
    {
        if (model == null) return null;

        return new ScheduleViewModel
        {
            ScheduleText = model.ToFriendlyScheduleText(true),
            StartDate = model.GetICalEvent()?.DtStart.Date,
            EndDate = model.GetICalEvent()?.DtEnd.Date,
            MeetingTime = model.GetICalEvent()?.DtStart.Value.TimeOfDay.ToString(@"hh\:mm"),
            RecurrenceRule = model.WithInterval(),
            iCalendarContent = model.iCalendarContent,
            Timezone = model.Timezone,
            Frequency = model.Frequency,
        };
    }
}