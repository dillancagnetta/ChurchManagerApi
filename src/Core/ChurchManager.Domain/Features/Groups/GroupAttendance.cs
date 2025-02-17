﻿using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Domain.Common;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Domain.Features.Groups
{
    [Table("GroupAttendance")]

    public class GroupAttendance : Entity<int>, IAggregateRoot<int>
    {
        /// <summary>
        /// Gets or sets the Id of the <see cref="Group"/> that the attendance is for.
        /// </summary>
        public int GroupId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public bool? DidNotOccur { get; set; }
        public int? AttendanceCount { get; set; }
        public int? FirstTimerCount { get; set; }
        public int? NewConvertCount { get; set; }
        public int? ReceivedHolySpiritCount { get; set; }
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Money"/> or offering for this attendance record
        /// </summary>
        public Money Offering { get; set; }

        /// <summary>
        /// Gets or sets the Id of the photos attached for this attendance
        /// </summary>
        public List<string> PhotoUrls { get; set; } = new();

        /// <summary>
        /// Gets or sets the <see cref="AttendanceReview"/> or feedback for this attendance records
        /// </summary>
        [Required]
        public AttendanceReview AttendanceReview { get; set; } = new();

        #region Navigation

        public virtual Group Group { get; set; }

        public virtual ICollection<GroupMemberAttendance> Attendees { get; set; } = new Collection<GroupMemberAttendance>();

        #endregion

        /// <summary>
        /// Gets a value indicating whether attendance was entered (based on presence of any attendee records).
        /// </summary>
        public virtual bool AttendanceEntered => Attendees.Any();

        /// <summary>
        /// Gets the number of attendees who attended.
        /// </summary>
        public virtual int DidAttendCount => Attendees.Count(a => a.DidAttend.HasValue && a.DidAttend.Value);

        /// <summary>
        /// Gets the attendance rate.
        /// </summary>
        /// <value>
        /// The attendance rate which is the number of attendance records marked as did attend
        /// divided by the total number of attendance records for this occurrence.
        /// </value>
        public double AttendanceRate
        {
            get
            {
                var totalCount = Attendees.Count;
                if(totalCount > 0)
                {
                    return (double)(DidAttendCount) / (double)totalCount;
                }

                return 0.0d;
            }
        }

        /// <summary>
        /// Gets a value indicating whether attendance was reviewed.
        /// </summary>
        public virtual bool AttendanceReviewed => AttendanceReview != null && AttendanceReview.IsReviewed.HasValue && AttendanceReview.IsReviewed.Value;
    }

    [Owned]
    public record AttendanceReview
    {
        public bool? IsReviewed { get; set; }
        public string Feedback { get; set; }
        [MaxLength(50)]
        public string ReviewedBy { get; set; }
    }
}
