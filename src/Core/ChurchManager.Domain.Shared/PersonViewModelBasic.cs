namespace ChurchManager.Domain.Shared;

public record PersonViewModelBasic
{
    public int PersonId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public string AgeClassification { get; set; }
    public string PhotoUrl { get; set; }
    public int? Age { get; set; }
}