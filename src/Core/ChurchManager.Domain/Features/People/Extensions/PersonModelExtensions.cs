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
            FirstName = person.FullName!.FirstName!,
            LastName = person.FullName!.LastName!,
            AgeClassification = person.AgeClassification,
            Age = person.BirthDate?.Age,
            PhotoUrl = person.PhotoUrl
        };
    }
}