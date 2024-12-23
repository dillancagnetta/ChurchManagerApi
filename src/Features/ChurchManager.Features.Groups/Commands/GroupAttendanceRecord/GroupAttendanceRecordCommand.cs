﻿using System.ComponentModel.DataAnnotations;
using ChurchManager.Features.Groups.Services;
using MediatR;

namespace ChurchManager.Features.Groups.Commands.GroupAttendanceRecord
{
    public record GroupAttendanceRecordCommand : IRequest<Unit>
    {
        [Required] public int GroupId { get; set; }

        [Required] public DateTime AttendanceDate { get; set; }

        public bool? DidNotOccur { get; set; }
        public IEnumerable<GroupMemberAttendance> Members { get; set; }
        public IEnumerable<FirstTimerAttendance> FirstTimers { get; set; }
        public string Notes { get; set; }
        public decimal? Offering { get; set; }
    }

    public class GroupAttendanceHandler : IRequestHandler<GroupAttendanceRecordCommand, Unit>
    {
        private readonly IGroupAttendanceAppService _appService;

        public GroupAttendanceHandler(IGroupAttendanceAppService appService)
        {
            _appService = appService;
        }

        public async Task<Unit> Handle(GroupAttendanceRecordCommand command, CancellationToken ct)
        {
            await _appService.RecordAttendanceAsync(command, ct);

            return new Unit();
        }
    }
}