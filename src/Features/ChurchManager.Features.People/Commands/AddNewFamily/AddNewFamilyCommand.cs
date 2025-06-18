using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Events;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Features.People.Specifications;
using ChurchManager.Infrastructure.Abstractions;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.SharedKernel.Common;
using CodeBoss.Extensions;
using Codeboss.Types;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Features.People.Commands.AddNewFamily
{
    public record AddNewFamilyCommand : IRequest<Unit>
    {
        public required string FamilyName { get; set; }
        public IEnumerable<FamilyMember> Members { get; set; } = [];
        public Address Address { get; set; } = new();
    }

    public class AddNewFamilyHandler : IRequestHandler<AddNewFamilyCommand, Unit>
    {
        private readonly IPersonDbRepository _dbRepository;
        private readonly IReadDbRepository<ConnectionStatusType> _connectionStatusDb;
        private readonly IGenericDbRepository<ConnectionStatusHistory> _connectionStatusHistoryDb;
        private readonly IDomainEventPublisher _eventPublisher;
        private readonly IAppCurrentUser _currentUser;
        private readonly IDateTimeProvider _dateTime;
        private readonly ILogger<AddNewFamilyHandler> _logger;

        public AddNewFamilyHandler(
            IPersonDbRepository dbRepository, 
            IReadDbRepository<ConnectionStatusType> connectionStatusDb, 
            IGenericDbRepository<ConnectionStatusHistory> connectionStatusHistoryDb, 
            IDomainEventPublisher eventPublisher,
            IAppCurrentUser currentUser,
            IDateTimeProvider dateTimeProvider,
            ILogger<AddNewFamilyHandler> logger)
        {
            _dbRepository = dbRepository;
            _connectionStatusDb = connectionStatusDb;
            _connectionStatusHistoryDb = connectionStatusHistoryDb;
            _eventPublisher = eventPublisher;
            _currentUser = currentUser;
            _dateTime = dateTimeProvider;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddNewFamilyCommand command, CancellationToken ct)
        {
            try
            {
                var family = new Family
                {
                    Name = $"{command.FamilyName}",
                    Address = new Domain.Features.People.Address
                    {
                        Street = command.Address.Street,
                        City = command.Address.City,
                        Province = command.Address.Province,
                        Country = command.Address.Country,
                        PostalCode = command.Address.PostalCode
                    }
                };

                /*
                 * https://stackoverflow.com/questions/42480952/cant-auto-generate-identity-with-addrange-in-entity-framework
                 *
                 *  Id's not showing after inserting: What was causing the problem? Enumerables!
                 *
                 *  Use:   ToArray()
                 */
                var members = command.Members.Select(x => new Person
                {
                    FullName = new FullName
                    {
                        FirstName = x.Person.FirstName,
                        MiddleName = x.Person.MiddleName,
                        LastName = x.Person.LastName
                    },
                    FirstVisitDate = x.FirstVisitDate,
                    ConnectionStatus = x.ConnectionStatus,
                    Gender = x.Person.Gender,
                    AgeClassification = x.Person.AgeClassification,
                    ReceivedHolySpirit = x.Person.ReceivedHolySpirit,
                    BirthDate = new Domain.Features.People.BirthDate
                    {
                        BirthDay = x.Person.BirthDate?.Day,
                        BirthMonth = x.Person.BirthDate?.Month,
                        BirthYear = x.Person.BirthDate?.Year
                    },
                    ChurchId = x.ChurchId,
                    Email = !x.Person.EmailAddress.IsNullOrEmpty()
                        ? new Email { Address = x.Person.EmailAddress!.Trim().ToLowerInvariant(), IsActive = true }
                        : null,
                    PhoneNumbers = x.Person.PhoneNumber != null
                        ? new List<PhoneNumber> { new()
                        {
                            CountryCode = x.Person.PhoneNumber.CountryCode, 
                            Number = x.Person.PhoneNumber.Number?.CleanPhoneNumber(), 
                            IsMessagingEnabled = x.Person.PhoneNumber.IsMessagingEnabled
                        } }
                        : null,
                    Source = x.Source,
                    Family = family
                }).ToArray(); // <----------------   the fix for the Id's

                await _dbRepository.AddRangeAsync(members, ct);
                await _dbRepository.SaveChangesAsync(ct);

                await SendFollowUpAssignments(command, members, ct);

                // Connection status history
                var connectionStatusTypes = await _connectionStatusDb.ListAsync(new ConnectionStatusSelectSpecification(), ct);
                var histories = new List<ConnectionStatusHistory>(members.Length);
                histories.AddRange(members.Select(person => new ConnectionStatusHistory
                {
                    PersonId = person.Id,
                    ConnectionStatusTypeId = connectionStatusTypes.First(x => x.Name == person.ConnectionStatus).Id,
                    StartDate = _dateTime.Now,
                    Notes = $"New family added"
                }));
                await _connectionStatusHistoryDb.AddRangeAsync(histories, ct);
                await _dbRepository.SaveChangesAsync(ct);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error adding new family command: {@command}", command);
                throw;
            }

            return new Unit();
        }

        private async Task SendFollowUpAssignments(AddNewFamilyCommand command, Person[] members, CancellationToken ct)
        {
            var followUpRequests = command.Members
                .Where(x => x.AssignedFollowUpPerson != null)
                .Select(x => new { x.Person, x.AssignedFollowUpPerson });

            var personFollowUps = members.Join(followUpRequests, // Join lists
                member => member.FullName!.FirstName + member.FullName.LastName + member.Email?.Address, // Join key
                followUp => followUp.Person!.FirstName + followUp.Person.LastName + followUp.Person.EmailAddress, // Join key
                (person, followUps) => new { Person = person, followUps.AssignedFollowUpPerson }); // Selection

            foreach (var followUp in personFollowUps)
            {
                await _eventPublisher.PublishAsync(
                    new FollowUpAssignedEvent(followUp.Person.Id, followUp.AssignedFollowUpPerson!.Id!.Value)
                    {
                        Type = $"{followUp.Person.ConnectionStatus}-{followUp.Person.Source}",
                        UserLoginId = _currentUser.Id
                    }, ct);
            }
        }
    }
}