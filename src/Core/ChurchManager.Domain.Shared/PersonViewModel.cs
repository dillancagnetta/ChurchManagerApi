using System.Collections.ObjectModel;

namespace ChurchManager.Domain.Shared;

public record PersonViewModel
{
    public int PersonId { get; set; }
    public int? FamilyId { get; set; }
    public ChurchViewModel? Church { get; set; }
    public string? ConnectionStatus { get; set; }
    public string? AgeClassification { get; set; }
    public string? Gender { get; set; }
    public DateTime? FirstVisitDate { get; set; }
    public FullNameViewModel? FullName { get; set; }
    public BirthDateViewModel? BirthDate { get; set; }
    public BaptismViewModel? BaptismStatus { get; set; }
    public string? MaritalStatus { get; set; }
    public DateTime? AnniversaryDate { get; set; }
    public EmailViewModel? Email { get; set; }
    public ICollection<PhoneNumberViewModel?> PhoneNumbers { get; set; } = new Collection<PhoneNumberViewModel?>();
    public string? CommunicationPreference { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Occupation { get; set; }
    public string? Source { get; set; }
    public bool? ReceivedHolySpirit { get; set; } = false;
    public DiscipleshipStepViewModel? FoundationSchool { get; set; }
    public ICollection<PersonViewModelBasic?> FamilyMembers { get; set; } = new Collection<PersonViewModelBasic?>();
    public string? RecordStatus { get; set; }
};


public record FullNameViewModel
{
    public string? Title { get; set; }
    public string? FirstName { get; set; }
    public string? NickName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Suffix { get; set; }

    public override string ToString() => $"{FirstName} {LastName}";
}

public record BaptismViewModel
{
    public bool? IsBaptised { get; set; }
    public DateTime? BaptismDate { get; set; }
}

public record PhoneNumberViewModel
{
    public int? Id { get; set; }
    public string? CountryCode { get; set; }
    public string? Number { get; set; }
    public string? Extension { get; set; }
    public string? Description { get; set; }
    public bool IsMessagingEnabled { get; set; }
    public bool IsUnlisted { get; set; }
}

public record EmailViewModel
{
    public string? Address { get; set; }
    public bool? IsActive { get; set; }
}