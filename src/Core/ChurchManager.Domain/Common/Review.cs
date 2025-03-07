using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Domain.Common;

[Owned]
public record Review
{
    public string ReviewerNote { get; set; }
    public DateTime? ReviewedDateTime { get; set; }
    public int? ReviewerPersonId { get; set; }
}