using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Churches;

[Table("ChurchAttendanceType", Schema = "Churches")]

public class ChurchAttendanceType : Entity<int>, IAggregateRoot<int>
{
    [Required, MaxLength(50)]
    public required string Name { get; set; }
    [MaxLength(100)]
    public string? Description { get; set; }
}