using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Groups.Commands.GroupTypes.Crud;

public record AddGroupTypeCommand : IRequest<ApiResponse>
{
    [Required] public string Name { get; set; }
    public string Description { get; set; }
    [Required] public string GroupTerm { get; set; }
    [Required] public string GroupMemberTerm { get; set; }
    [Required] public bool TakesAttendance { get; set; }
    [Required] public bool IsSystem { get; set; }
    public string IconCssClass { get; set; }
}

public class GroupTypeAddedHandler : IRequestHandler<AddGroupTypeCommand, ApiResponse>
{
    private readonly IGenericDbRepository<GroupType> _repository;

    public GroupTypeAddedHandler(IGenericDbRepository<GroupType> repository)
    {
        _repository = repository;
    }
    
    public async Task<ApiResponse> Handle(AddGroupTypeCommand cmd, CancellationToken ct)
    {
        var added = await _repository.AddAsync(new GroupType()
        {
            Name = cmd.Name,
            Description = cmd.Description,
            GroupTerm = cmd.GroupTerm,
            GroupMemberTerm = cmd.GroupMemberTerm,
            IconCssClass = cmd.IconCssClass,
            TakesAttendance = cmd.TakesAttendance,
            IsSystem = cmd.IsSystem,
        }, ct);
        
        return new ApiResponse(added);
    }
}