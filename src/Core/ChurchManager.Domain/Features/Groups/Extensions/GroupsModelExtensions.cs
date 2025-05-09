using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.Groups.Extensions;

public static class GroupModelExtensions
{
    public static GroupViewModel? ToModel(this Group? model)
    {
        if (model == null) return null;

        return new GroupViewModel
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            Address = model.Address,
            StartDate = model.StartDate,
            ChurchId = model.ChurchId,
            ChurchName = model.Church?.Name,
            ParentGroupId = model.ParentGroupId,
            ParentGroupChurchId = model.ParentGroup?.ChurchId,
            ParentGroupTypeId = model.ParentGroup?.GroupTypeId,
            ParentGroupName = model.ParentGroup?.Name,
            IsOnline = model.IsOnline,
            GroupType = model.GroupType.ToModel(),
            CreatedDate = model.CreatedDate,
            Schedule = model.Schedule.ToModel(),
            Level = 0, // We'll set this during tree building
            Groups = new List<GroupViewModel>(), // We'll populate this during tree building
        };

    }
    
    public static GroupTypeViewModel? ToModel(this GroupType? model)
    {
        if (model == null) return null;

        return new GroupTypeViewModel
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            GroupTerm = model.GroupTerm,
            GroupMemberTerm = model.GroupMemberTerm,
            TakesAttendance = model.TakesAttendance,
            IsSystem = model.IsSystem,
            IconCssClass = model.IconCssClass,
        };
    }
}