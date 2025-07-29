namespace ChurchManager.Domain.Shared;


public record JobViewModel
{
    public int Id { get; set; }
    public Guid JobKey { get; set; }
    public required string Name { get; set; }
        
    public string? Description { get; set; }
    public string? Assembly { get; set; }
    public required string Class { get; set; }
    public required string CronExpression { get; set; }
    public required string CronDescription { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastSuccessfulRunDateTime { get; set; }
    public DateTime? LastRunDateTime { get; set; }
    
    public int? LastRunDurationSeconds { get; set; }
    public string? LastStatus { get; set; }
    public string? LastStatusMessage { get; set; }
    public Dictionary<string, string> JobParameters { get; set; } = new();
    public string? NotificationEmails { get; set; }
    public bool EnableHistory { get; set; } = false;
    public int HistoryCount { get; set; } = 500;
    public string? NotificationStatus { get; set; }

    public IEnumerable<JobHistoryViewModel> History { get; set; } = [];
}

public record JobHistoryViewModel
{
    public int Id { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? StopDateTime { get; set; }
    public string? Status { get; set; }
    public string? StatusMessage { get; set; }
}

public record EditJobViewModel
{
    public string? Description { get; set; }
    public string? Assembly { get; set; }
    public string? Class { get; set; }
    public string? CronExpression { get; set; }
    public string? CronDescription { get; set; }
    public bool IsActive { get; set; }
}