using System.Linq.Expressions;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Churches.Specifications;

public static class ExpressionExtensions
{
    public static Expression<Func<ChurchAttendance, ChurchAttendanceViewModel>> SelectChurchAttendance = x =>
        new ChurchAttendanceViewModel
        {
            Id = x.Id,
            ChurchName = x.Church.Name,
            ChurchGroupName = x.Church.ChurchGroup.Name,
            AttendanceTypeName = x.ChurchAttendanceType.Name,
            AttendanceDate = x.AttendanceDate,
            DidNotOccur = x.DidNotOccur,
            AttendanceCount = x.AttendanceCount,
            FirstTimerCount = x.FirstTimerCount,
            NewConvertCount = x.NewConvertCount,
            ReceivedHolySpiritCount = x.ReceivedHolySpiritCount,
            MalesCount = x.MalesCount,
            FemalesCount = x.FemalesCount,
            ChildrenCount = x.ChildrenCount,
            TeensCount = x.TeensCount,
            Notes = x.Notes,
            PhotoUrls = x.PhotoUrls,
        };
}