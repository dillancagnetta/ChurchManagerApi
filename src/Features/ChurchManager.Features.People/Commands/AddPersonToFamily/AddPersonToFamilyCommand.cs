﻿using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Features.People.Commands.AddNewFamily;
using CodeBoss.Extensions;
using MediatR;
using BirthDate = ChurchManager.Domain.Features.People.BirthDate;

namespace ChurchManager.Features.People.Commands.AddPersonToFamily
{
    public record AddPersonToFamilyCommand : IRequest<Unit>
    {
        public FamilyMember FamilyMember { get; set; }
    }

    public class AddPersonToFamilyHandler : IRequestHandler<AddPersonToFamilyCommand, Unit>
    {
        private readonly IPersonDbRepository _dbRepository;

        public AddPersonToFamilyHandler(IPersonDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<Unit> Handle(AddPersonToFamilyCommand command, CancellationToken ct)
        {
            var member = command.FamilyMember ?? throw new NullReferenceException(nameof(command.FamilyMember));

            var person = new Person
            {
                FullName = new FullName
                {
                    FirstName = member.Person.FirstName,
                    MiddleName = member.Person.MiddleName,
                    LastName = member.Person.LastName
                },
                FirstVisitDate = member.FirstVisitDate,
                ConnectionStatus = member.ConnectionStatus,
                Gender = member.Person.Gender,
                AgeClassification = member.Person.AgeClassification,
                ReceivedHolySpirit = member.Person.ReceivedHolySpirit,
                BirthDate = new BirthDate
                {
                    BirthDay = member.Person.BirthDate?.Day,
                    BirthMonth = member.Person.BirthDate?.Month,
                    BirthYear = member.Person.BirthDate?.Year
                },
                ChurchId = member.ChurchId,
                Email = !member.Person.EmailAddress.IsNullOrEmpty()
                    ? new Email { Address = member.Person.EmailAddress, IsActive = true }
                    : null,
                PhoneNumbers = !member.Person.PhoneNumber.IsNullOrEmpty()
                    ? new List<PhoneNumber> { new() { CountryCode = "+27", Number = member.Person.PhoneNumber } }
                    : null,
                Source = member.Source,
                FamilyId = member.FamilyId,  // Assign to the family
            };

            await _dbRepository.AddAsync(person, ct);

            return Unit.Value;
        }
    }
}
