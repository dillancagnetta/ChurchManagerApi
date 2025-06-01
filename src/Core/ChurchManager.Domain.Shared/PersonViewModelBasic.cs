namespace ChurchManager.Domain.Shared;

public record PersonViewModelBasic
{
    public int PersonId { get; set; }
    public string? Title { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Gender { get; set; }
    public string? AgeClassification { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Email { get; set; }
    public int? Age { get; set; }
    public BirthDateViewModel? BirthDate { get; set; }
}

public record BirthDateViewModel
{
    public int? BirthDay { get; set; }
    public int? BirthMonth { get; set; }
    public int? BirthYear { get; set; }
    public int? Age { get; set; }
}
