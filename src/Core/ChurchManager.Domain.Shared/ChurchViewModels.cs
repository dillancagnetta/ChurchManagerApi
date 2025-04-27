namespace ChurchManager.Domain.Shared;

public record ChurchViewModel : SelectItemViewModel
{
    public string? ShortCode { get; set; }
    public int ChurchGroupId { get; set; }
    public PersonViewModelBasic? LeaderPerson { get; set; }
}

public record ChurchGroupViewModel : SelectItemViewModel
{
    public IEnumerable<ChurchViewModel> Churches { get; set; } = [];
    public PersonViewModelBasic? LeaderPerson { get; set; }
}

// -------------------

public record EditChurchModel : SelectItemViewModel
{
    public int? ChurchGroupId { get; set; }
    public string? ShortCode { get; set; }
    public int? LeaderPersonId { get; set; }
}

public record EditChurchGroupModel : SelectItemViewModel
{
    public int? LeaderPersonId { get; set; }
}