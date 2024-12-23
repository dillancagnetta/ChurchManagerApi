﻿using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Missions;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using MediatR;

namespace ChurchManager.Features.Missions.Commands.AddMission
{
    public record AddMissionCommand : IRequest<Unit>
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        [MaxLength(100)]
        public string Type { get; set; } // InReach, OutReach etc

        [MaxLength(200)]
        public string Category { get; set; } // ROSA, Healing Streams etc

        /// <summary>
        /// Gets or sets the icon CSS class.
        /// </summary>
        [MaxLength(100)]
        public string IconCssClass { get; set; } = "heroicons_solid:calendar";

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// Optional Person who headed this mission
        /// </summary>
        public int? PersonId { get; set; }

        /// <summary>
        /// Optional Church who headed this mission
        /// </summary>
        public int? ChurchId { get; set; }

        /// <summary>
        /// Optional Group who headed this mission
        /// </summary>
        public int? GroupId { get; set; }

        public Attendance Attendance { get; set; }

        public string Notes { get; set; }
    }

    public class AddMissionHandler : IRequestHandler<AddMissionCommand, Unit>
    {
        private readonly IGenericDbRepository<Mission> _dbRepository;

        public AddMissionHandler(IGenericDbRepository<Mission> dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<Unit> Handle(AddMissionCommand command, CancellationToken ct)
        {
            var entity = Map(command);

            // Assign Mission Stream
            var stream = command.PersonId.HasValue ? "Person" : string.Empty;
            stream = command.GroupId.HasValue ? "Group" : stream;
            stream = command.ChurchId.HasValue ? "Church" : stream;
            entity.Stream = stream;

            await _dbRepository.AddAsync(entity, ct);

            return Unit.Value;
        }

        public Mission Map(AddMissionCommand entity)
        {
            return new()
            {
                Name = entity.Name,
                Description = entity.Description,
                Type = entity.Type,
                Category = entity.Category,
                IconCssClass = entity.IconCssClass,
                StartDateTime = entity.StartDateTime,
                EndDateTime = entity.EndDateTime,
                PersonId = entity.PersonId,
                ChurchId = entity.ChurchId,
                GroupId = entity.GroupId,
                Attendance = entity.Attendance,
                Notes = entity.Notes,
                RecordStatus = RecordStatus.Active
            };
        }
    }
}
