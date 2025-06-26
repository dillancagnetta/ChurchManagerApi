using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Domain.Features.Churches;

[Table("ChurchServiceTime", Schema = "Churches")]
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