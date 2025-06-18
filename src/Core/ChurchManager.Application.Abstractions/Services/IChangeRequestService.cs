using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.ChangeRequests;
using Codeboss.Results;

namespace ChurchManager.Application.Abstractions.Services;

public interface IChangeRequestService
{
    Task<OperationResult<ChangeRequest>> CreatePersonChangeRequestAsync(int personId,
        Dictionary<string, object?> propertyChanges, string? reason = null);

    Task<OperationResult<ChangeRequest>> CreateBaptismChangeRequestAsync(int personId,
        bool? isBaptised, DateTime? baptismDate = null, string? reason = null);

    Task<OperationResult<ChangeRequest>> CreateHolySpiritChangeRequestAsync(int personId,
        bool receivedHolySpirit, string? reason = null);
}