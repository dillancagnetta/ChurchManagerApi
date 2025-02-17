﻿using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Churches
{
    [Table("ChurchAttendance")]

    public class ChurchAttendance : Entity<int>, IAggregateRoot<int>
    {
        public int ChurchAttendanceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the <see cref="Church"/> that the attendance is for.
        /// </summary>
        public int ChurchId { get; set; }

        public DateTime AttendanceDate { get; set; }

        public int AttendanceCount { get; set; }
        public int? MalesCount { get; set; }
        public int? FemalesCount { get; set; }
        public int? ChildrenCount { get; set; }
        public int? FirstTimerCount { get; set; }
        public int? NewConvertCount { get; set; }
        public int? ReceivedHolySpiritCount { get; set; }
        public string Notes { get; set; }

        #region Navigation

        public virtual ChurchAttendanceType ChurchAttendanceType { get; set; }

        #endregion
    }
}
