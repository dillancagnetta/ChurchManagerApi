﻿using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Repositories;
using MediatR;

namespace ChurchManager.Features.People.Commands.UpdatePerson
{
    public record UpdatePersonalInfoCommand : IRequest<Unit>
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string AgeClassification { get; set; }
    }

    public class UpdatePersonalInfoCommandHandler : IRequestHandler<UpdatePersonalInfoCommand, Unit>
    {
        private readonly IPersonDbRepository _dbRepository;

        public UpdatePersonalInfoCommandHandler(IPersonDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<Unit> Handle(UpdatePersonalInfoCommand command, CancellationToken ct)
        {
            var person = await _dbRepository.GetByIdAsync(command.PersonId, ct) ??
                         throw new ArgumentNullException(nameof(Person));
            person.FullName.FirstName = command.FirstName;
            person.FullName.MiddleName = command.MiddleName;
            person.FullName.LastName = command.LastName;
            person.Gender = command.Gender;
            person.AgeClassification = command.AgeClassification;


            await _dbRepository.SaveChangesAsync(ct);

            return new Unit();
        }
    }
}