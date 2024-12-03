namespace ChurchManager.Domain.Shared;

public record ChurchViewModel : SelectItemViewModel
{
    public string ShortCode { get; set; }
    public PeopleAutocompleteViewModel LeaderPerson { get; set; }
}

public record ChurchGroupViewModel : SelectItemViewModel
{
    public string ShortCode { get; set; }
    public IEnumerable<ChurchViewModel> Churches { get; set; }
    public PeopleAutocompleteViewModel LeaderPerson { get; set; }
}