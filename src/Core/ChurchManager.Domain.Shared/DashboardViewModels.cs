namespace ChurchManager.Domain.Shared;

public record CountItemViewModel
{
    public required string Name { get; set; }
    public int Count { get; set; }
}

public record StatisticsViewModel
{
    public Dictionary<string, List<CountItemViewModel>> Data { get; set; } = new();
}