using ChurchManager.Domain.Features.Groups;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.Groups.Commands.AddGroupMember
{
    public record AddGroupMembersCommand : IRequest<ApiResponse>
    {
        public int GroupId { get; set; }
        public IList<int> PersonIds { get; set; } = [];
        public int GroupRoleId { get; set; }
        public string RecordStatus { get; set; } = "Active";
    }

    public class GroupMembersAddedHandler : IRequestHandler<AddGroupMembersCommand, ApiResponse>
    {
        private readonly IGenericDbRepository<GroupMember> _dbRepository;

        public GroupMembersAddedHandler(IGenericDbRepository<GroupMember> dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<ApiResponse> Handle(AddGroupMembersCommand command, CancellationToken ct)
        {
            // Check they are not a group member already
            var existingPersonIds = await _dbRepository.Queryable().AsNoTracking()
			.Where(m => m.GroupId == command.GroupId && command.PersonIds.Contains(m.PersonId))
			.Select(m => m.PersonId)
			.ToListAsync(ct);

			var newPersonIds = command.PersonIds.Except(existingPersonIds).ToList();

            var groupMembers = newPersonIds.Select(personId => new GroupMember
			{
				PersonId = personId,
				GroupId = command.GroupId,
				GroupRoleId = command.GroupRoleId
			}).ToList();

			var added = await _dbRepository.AddRangeAsync(groupMembers, ct);
            return new ApiResponse(true);
        }
    }
}