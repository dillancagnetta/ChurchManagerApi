using ChurchManager.Domain.Features.Groups;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Features.Groups.Commands.GroupTypes.Crud;

public record DeleteGroupTypeCommand(int GroupTypeId) : IRequest<ApiResponse>;

public class GroupTypeDeletedHandler : IRequestHandler<DeleteGroupTypeCommand, ApiResponse>
{
    private readonly IGenericDbRepository<GroupType> _repository;
    private readonly ILogger<GroupTypeDeletedHandler> _logger;

    public GroupTypeDeletedHandler(IGenericDbRepository<GroupType> repository,  ILogger<GroupTypeDeletedHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ApiResponse> Handle(DeleteGroupTypeCommand cmd, CancellationToken cts)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(cmd.GroupTypeId, cts);
            await _repository.DeleteAsync(entity!, cts);
        }
        catch (Exception ex)
        {
            var message = $"Problem group type with Id: {cmd.GroupTypeId}";
            _logger.LogError(ex, message);
            return new ApiResponse(message);
        }
        
        return new ApiResponse(true);
    }
}