namespace ChurchManager.Domain.Shared;

public record ChurchViewModel : SelectItemViewModel
{
    public string? ShortCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public int ChurchGroupId { get; set; }
    public PersonViewModelBasic? LeaderPerson { get; set; }
    public IEnumerable<ChurchServiceTimeViewModel> ServiceTimes { get; set; } = [];
}

public record ChurchGroupViewModel : SelectItemViewModel
{
    public IEnumerable<ChurchViewModel>? Churches { get; set; } = [];
    public PersonViewModelBasic? LeaderPerson { get; set; }
}

// -------------------

public record EditChurchModel : SelectItemViewModel
{
    public int? ChurchGroupId { get; set; }
    public string? ShortCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public int? LeaderPersonId { get; set; }
    public ICollection<ChurchServiceTimeViewModel> ServiceTimes { get; set; } = [];
}

public record EditChurchGroupModel : SelectItemViewModel
{
    public int? LeaderPersonId { get; set; }
}

public record ChurchServiceTimeViewModel
{
    public int? Id { get; set; }
    public string? ChurchAttendanceType { get; set; }
    public int ChurchAttendanceTypeId { get; set; }
    public string? DayOfWeek { get; set; }
    public TimeOnly Time { get; set; }
}