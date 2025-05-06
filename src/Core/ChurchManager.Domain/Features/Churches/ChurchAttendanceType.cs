using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Churches;

[Table("ChurchAttendanceType")]

public class ChurchAttendanceType : Entity<int>, IAggregateRoot<int>
{
    [Required, MaxLength(50)]
    public required string Name { get; set; }
    [MaxLength(100)]
    public string? Description { get; set; }
}

[Table("ChurchServiceTime")]
public class ChurchServiceTime : Entity<int>
{
    public int ChurchId { get; set; }
    public int ChurchAttendanceTypeId { get; set; }
    
    [Required, MaxLength(50)]
    public string? DayOfWeek { get; set; }
    
    public TimeOnly Time { get; set; }
    
    #region Navigation Properties
    
    public virtual Church? Church { get; set; }
    public virtual ChurchAttendanceType? ChurchAttendanceType { get; set; }

    #endregion
}