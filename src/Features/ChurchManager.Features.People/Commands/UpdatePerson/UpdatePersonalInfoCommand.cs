using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.SharedKernel.Wrappers;
using CodeBoss.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Features.People.Commands.UpdatePerson
{
    public record UpdatePersonalInfoCommand : IRequest<ApiResponse>
    {
        public int PersonId { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? AgeClassification { get; set; }
        
        // Additional from Public
        public string? Title { get; set; }
        public string? Occupation { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? MaritalStatus { get; set; }
        public BirthDateViewModel? BirthDate { get; set; }
    }

    public class UpdatePersonalInfoCommandHandler : IRequestHandler<UpdatePersonalInfoCommand, ApiResponse>
    {
        private readonly IPersonDbRepository _dbRepository;

        public UpdatePersonalInfoCommandHandler(IPersonDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<ApiResponse> Handle(UpdatePersonalInfoCommand command, CancellationToken ct)
        {
            var person = await _dbRepository.Queryable().Include(x => x.PhoneNumbers)
                             .Where(x => x.Id == command.PersonId)
                             .FirstOrDefaultAsync(ct) ?? throw new ArgumentNullException(nameof(Person));
            
            person.FullName!.Title = command.Title;
            person.FullName!.FirstName = command.FirstName;
            person.FullName.MiddleName = command.MiddleName;
            person.FullName.LastName = command.LastName;
            person.Gender = command.Gender ?? Gender.Unknown;
            person.AgeClassification = command.AgeClassification ?? AgeClassification.Unknown;

            // Additional from Public
            person.Occupation = command.Occupation;
            person.Email = !command.Email.IsNullOrEmpty() ? Email.Create(command.Email!) : null;
            person.MaritalStatus = command.MaritalStatus;

            if (person.BirthDate != null && command.BirthDate != null)
            {
                person.BirthDate.Update(command.BirthDate.BirthDay, command.BirthDate.BirthMonth, command.BirthDate.BirthYear);
            } 
            else
            {
                person.BirthDate = BirthDate.Create(command.BirthDate?.BirthDay, command.BirthDate?.BirthMonth, command.BirthDate?.BirthYear);
            }

            var phoneNumber = person.PhoneNumbers?.FirstOrDefault();
            if (phoneNumber == null && !command.PhoneNumber.IsNullOrEmpty())
            {
                person.PhoneNumbers = new List<PhoneNumber> { new() { CountryCode = "+27", Number = command.PhoneNumber } };
            }
            else if (phoneNumber != null && !command.PhoneNumber.IsNullOrEmpty())
            {
                phoneNumber.Number = command.PhoneNumber;
            }
            
            await _dbRepository.SaveChangesAsync(ct);

            return ApiResponse.Success();
        }
    }
}