namespace ChurchManager.Domain.Shared;
using PersonViewModel = ChurchManager.Domain.Shared.PersonViewModelBasic;

public record ChurchViewModel : SelectItemViewModel
{
    public string ShortCode { get; set; }
    public PersonViewModel LeaderPerson { get; set; }
}

public record ChurchGroupViewModel : SelectItemViewModel
{
    public IEnumerable<ChurchViewModel> Churches { get; set; }
    public PersonViewModel LeaderPerson { get; set; }
}