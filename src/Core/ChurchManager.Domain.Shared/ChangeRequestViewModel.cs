namespace ChurchManager.Domain.Shared;

public record ChangeRequestViewModel
{
    public int Id { get; set; }
    public DateTime RequestedDate { get; set; } 
    public string? Reason { get; set; }
    public string? Source { get; set; }
    public string Status { get; set; }
    // Review
    public PersonViewModelBasic? ReviewedByPerson { get; set; }
    public DateTime? ReviewedDate { get; set; }
    public string? ReviewNotes { get; set; }
    
    // Entity
    public string? EntityType { get; set; } // "Person", "Family", "Church", etc.
    public int? EntityId { get; set; }
    public PersonViewModelBasic? PersonEntity { get; set; }
    
    public IEnumerable<PropertyChangeRequestViewModel> Properties { get; set; } = new List<PropertyChangeRequestViewModel>();
};

public record PropertyChangeRequestViewModel
{
    public int Id { get; set; }
    public string PropertyPath { get; set; } // e.g., "BaptismStatus.IsBaptised", "ReceivedHolySpirit", "FullName.FirstName"
    public int EntityId { get; set; }
    public string? CurrentValue { get; set; } // JSON serialized current value
    public string? RequestedValue { get; set; } // JSON serialized requested value
    public string? PropertyType { get; set; } // For deserialization hints
    public bool IsApplied { get; set; }
}