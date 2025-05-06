namespace ChurchManager.Domain.Shared;

public record CommunicationViewModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Subject { get; set; }
    public required string Content { get; set; }
    public string? Category { get; set; }
    public required string CommunicationType  { get;  set; }
    public GroupReference? ListGroup { get;  set; }
    public int? CommunicationTemplateId { get;  set; }
    public string? CommunicationContent { get; set; }
    public PersonViewModelBasic? SenderPerson { get;   set; }
    public bool IsBulkCommunication { get;  set; }
    public DateTime? SendDateTime { get;  set; }
    public DateTime? FutureSendDateTime { get;  set; }
    public DateTime? CreatedDateTime { get;  set; }
    public required string Status { get;  set; }
    public CommunicationReviewViewModel? Review { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
    public int? SystemCommunicationId { get; set; }
    public int RecipientCount { get; set; }
}


public record CommunicationReviewViewModel
{
    public string? ReviewerNote { get; set; }
    public DateTime? ReviewedDateTime { get; set; }
    public int? ReviewerPersonId { get; set; }
}

public record CommunicationRecipientViewModel
{
    public PersonViewModelBasic? RecipientPerson { get;   set; }
    public string? Status { get; set; }
    public string? StatusNote { get; set; }
    public DateTime? SendDateTime { get; set; }
    public DateTime? OpenedDateTime { get; set; }
    public string? UniqueMessageId { get; set; }
    public int AttemptCount  { get; set; }
}