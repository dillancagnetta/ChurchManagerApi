namespace ChurchManager.Application.ViewModels;

public record HistoryViewModel
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Category { get; set; }
    public string Entity { get; set; }
    public string Verb { get; set; }
    public bool IsSensitive { get; set; }
    public string Caption { get; set; }
    public string RelatedData { get; set; }
    public string RelatedEntity { get; set; }
    public int? RelatedEntityId { get; set; }
    public string ValueName { get; set; }
    public string NewValue { get; set; }
    public string OldValue { get; set; }
}