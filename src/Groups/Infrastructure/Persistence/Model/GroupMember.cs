﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Codeboss.Types;

namespace Infrastructure.Persistence.Model
{
    [Table("GroupMember", Schema = "Groups")]

    public class GroupMember : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the <see cref="Person"/> that is represented by the GroupMember. This property is required.
        /// </summary>
        public int PersonId { get; set; }

        public int GroupMemberRoleId { get; set; }

        public string GroupMemberStatus { get; set; } = "Active";

        /// <summary>
        /// Gets or sets the date/time that the person was added to the group.
        /// Should automatically set this value when a group member is added if it isn't set manually
        /// </summary>
        public DateTime? DateTimeAdded { get; set; }

        /// <summary>
        /// Gets or sets the date that this group member became inactive
        /// </summary>
        public DateTime? InactiveDateTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this group member is archived (soft deleted)
        /// </summary>
        public bool IsArchived { get; set; } = false;

        /// <summary>
        /// Gets or sets the date time that this group member was archived (soft deleted)
        /// </summary>
        /// <value>
        public DateTime? ArchivedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the communication preference.
        /// </summary>
        public string CommunicationPreference { get; set; } = "Message";

        public virtual Group Group { get; set; }
        public virtual GroupMemberRole GroupMemberRole { get; set; }
    }

    #region Enumerations

    /// <summary>
    /// Represents the status of a <see cref="GroupMember"/> in a <see cref="Group"/>.
    /// </summary>
    public class GroupMemberStatus : Enumeration<GroupMemberStatus, string>
    {
        public GroupMemberStatus(string value) => Value = value;

        /// <summary>
        /// The <see cref="GroupMember"/> is not an active member of the <see cref="Group"/>.
        /// </summary>
        public static GroupMemberStatus Inactive = new( "Inactive");


        /// <summary>
        /// The <see cref="GroupMember"/> is an active member of the <see cref="Group"/>.
        /// </summary>
        public static GroupMemberStatus Active = new("Active");


        /// <summary>
        /// The <see cref="GroupMember">GroupMember's</see> membership in the <see cref="Group"/> is pending.
        /// </summary>
        public static GroupMemberStatus Pending = new("Pending");
    }

    #endregion


}
