﻿using ChurchManager.Domain.Features.People.Repositories;
using MediatR;

namespace ChurchManager.Features.People.Commands.UpdatePerson
{
    public record UpdateConnectionInfoCommand : IRequest<Unit>
    {
        public int PersonId { get; set; }
        public int ChurchId { get; set; }
        public string ConnectionStatus { get; set; }
        public DateTime? FirstVisitDate { get; set; }
        public string Source { get; set; }
    }

    public class UpdateConnectionInfoHandler : IRequestHandler<UpdateConnectionInfoCommand, Unit>
    {
        private readonly IPersonDbRepository _dbRepository;

        public UpdateConnectionInfoHandler(IPersonDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<Unit> Handle(UpdateConnectionInfoCommand command, CancellationToken ct)
        {
            var person = await _dbRepository.GetByIdAsync(command.PersonId, ct);

            if (person is not null)
            {
                person.ChurchId = command.ChurchId;
                person.ConnectionStatus = command.ConnectionStatus;
                person.FirstVisitDate = command.FirstVisitDate;
                person.Source = command.Source;

                await _dbRepository.SaveChangesAsync(ct);
            }

            return new Unit();
        }
    }
}