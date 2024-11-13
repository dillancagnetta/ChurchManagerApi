using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Groups.Commands.GroupTypes.Crud;

public record EditGroupTypeCommand : IRequest<ApiResponse>
{
    [Required] public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string GroupTerm { get; set; }
    public string GroupMemberTerm { get; set; }
    public bool TakesAttendance { get; set; }
    public bool IsSystem { get; set; }
    public string IconCssClass { get; set; }
}

public class GroupTypeEditedHandler : IRequestHandler<EditGroupTypeCommand, ApiResponse>
{
    private readonly IGenericDbRepository<GroupType> _repository;

    public GroupTypeEditedHandler(IGenericDbRepository<GroupType> repository)
    {
        _repository = repository;
    }
    
    public async Task<ApiResponse> Handle(EditGroupTypeCommand cmd, CancellationToken cts)
    {
        var entity = await _repository.GetByIdAsync(cmd.Id, cts);

        entity.Name = cmd.Name;
        entity.Description = cmd.Description;
        entity.GroupTerm = cmd.GroupTerm;
        entity.GroupMemberTerm = cmd.GroupMemberTerm;
        entity.IconCssClass = cmd.IconCssClass;
        entity.TakesAttendance = cmd.TakesAttendance;
        entity.IsSystem = cmd.IsSystem;
        
        await _repository.SaveChangesAsync();
        
        return new ApiResponse(entity);
    }
}