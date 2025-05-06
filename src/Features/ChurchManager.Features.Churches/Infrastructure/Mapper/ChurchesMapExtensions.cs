using ChurchManager.Domain.Shared;
using ChurchManager.Features.Churches.Commands;

namespace ChurchManager.Features.Churches.Infrastructure.Mapper;

public static class ChurchesMapExtensions
{
    public static EditChurchModel ToModel(this EditChurchCommand command)
    {
        return new EditChurchModel
        {
            Id = command.Id,
            Name = command.Name,
            Description = command.Description,
            LeaderPersonId = command.LeaderPersonId,
            ChurchGroupId = command.ChurchGroupId,
            ShortCode = command.ShortCode,
            PhoneNumber = command.PhoneNumber,
            Address = command.Address,
            ServiceTimes = command.ServiceTimes
        };
    }
}