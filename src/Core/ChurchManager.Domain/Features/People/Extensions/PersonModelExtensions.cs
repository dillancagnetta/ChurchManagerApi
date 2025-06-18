using ChurchManager.Domain.Features.Churches.Extensions;
using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Domain.Features.People.Extensions;

public static class PersonModelExtensions
{
    public static PersonViewModelBasic? ToBasicPersonViewModel(this Person? person)
    {
        if (person == null) return null;
        
        return new PersonViewModelBasic
        {
            PersonId = person.Id,
            Gender = person.Gender,
            Title = person.FullName!.Title,
            FirstName = person.FullName!.FirstName!,
            LastName = person.FullName!.LastName!,
            AgeClassification = person.AgeClassification,
            Age = person.BirthDate?.Age,
            PhotoUrl = person.PhotoUrl,
            BirthDate = person.BirthDate?.ToViewModel(),
            Email = person.Email?.Address
        };
    }
    
    /// <summary>
        /// Maps a Person entity to a PersonViewModel
        /// </summary>
        /// <param name="person">The person entity to map</param>
        /// <param name="condensed">Whether to include family members in the mapping</param>
        /// <returns>A PersonViewModel representing the person</returns>
        public static PersonViewModel? ToViewModel(this Person? person, bool condensed = false)
        {
            if (person == null) return null;

            return new PersonViewModel
            {
                PersonId = person.Id,
                FamilyId = person.FamilyId,
                Church = person.Church?.ToViewModel(false),
                ConnectionStatus = person.ConnectionStatus?.Value,
                AgeClassification = person.AgeClassification?.Value,
                CommunicationPreference = person.CommunicationPreference == null 
                    ? CommunicationType.None.Value 
                    : person.CommunicationPreference.Value,
                FamilyMembers = condensed || person.Family == null
                    ? new List<PersonViewModelBasic?>(0)
                    : person.Family.FamilyMembers
                        .Select(fm => fm?.ToBasicPersonViewModel())
                        .OrderBy(fm => fm?.FirstName)
                            .ThenBy(fm => fm?.AgeClassification)
                        .ToList(),
                Gender = person.Gender?.Value,
                FirstVisitDate = person.FirstVisitDate,
                FullName = person.FullName?.ToViewModel(),
                BirthDate = person.BirthDate?.ToViewModel(),
                BaptismStatus = person.BaptismStatus?.ToViewModel(),
                MaritalStatus = person.MaritalStatus,
                AnniversaryDate = person.AnniversaryDate,
                Email = person.Email?.ToViewModel(),
                PhoneNumbers = person.PhoneNumbers == null 
                    ? new List<PhoneNumberViewModel?>(0)
                    : person.PhoneNumbers.Select(pn => pn.ToViewModel()).ToList(),
                PhotoUrl = person.PhotoUrl,
                Occupation = person.Occupation,
                Source = person.Source,
                ReceivedHolySpirit = person.ReceivedHolySpirit,
                RecordStatus = person.RecordStatus,
            };
        }
    
    public static BirthDateViewModel? ToViewModel(this BirthDate? birthDate)
    {
        if (birthDate == null) return null;
        
        return new BirthDateViewModel
        {
            BirthDay = birthDate.BirthDay,
            BirthMonth = birthDate.BirthMonth,
            BirthYear = birthDate.BirthYear,
            Age = birthDate.Age
        };
    }
    
    public static FullNameViewModel? ToViewModel(this FullName? model)
    {
        if (model == null) return null;
        
        return new FullNameViewModel
        {
            Title = model.Title,
            FirstName = model.FirstName,
            MiddleName = model.MiddleName,
            LastName = model.LastName,
            Suffix = model.Suffix,
            NickName = model.NickName
        };
    }
    
    public static BaptismViewModel? ToViewModel(this Baptism? model)
    {
        if (model == null) return null;
        
        return new BaptismViewModel
        {
            BaptismDate = model.BaptismDate,
            IsBaptised = model.IsBaptised,
        };
    }
    
    public static PhoneNumberViewModel? ToViewModel(this PhoneNumber? model)
    {
        if (model == null) return null;
        
        return new PhoneNumberViewModel
        {
            Id = model.Id,
            Number = model.Number,
            CountryCode = model.CountryCode,
            Description = model.Description,
            Extension = model.Extension,
            IsMessagingEnabled = model.IsMessagingEnabled,
            IsUnlisted = model.IsUnlisted,
        };
    }
    
    public static EmailViewModel? ToViewModel(this Email? model)
    {
        if (model == null) return null;
        
        return new EmailViewModel
        {
           Address = model.Address,
           IsActive = model.IsActive,
        };
    }
}