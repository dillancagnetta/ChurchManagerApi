﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Shared.Persistence;
using Codeboss.Types;

namespace Churches.Persistence.Models
{
    [Table("Church", Schema = "Churches")]

    public class Church : Entity<int>, IAggregateRoot<int>
    {
        public int? ChurchGroupId { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        [MaxLength(10)]
        public string ShortCode { get; set; }
        [MaxLength(15)]
        public string PhoneNumber { get; set; }
        [MaxLength(200)]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the Id of the <see cref="Person"/> that is the leader of the Church.
        /// </summary>
        public int? LeaderPersonId { get; set; }

        #region Navigation

        /// <summary>
        /// Gets or sets  the <see cref="ChurchGroup">ChurchGroup</see> that this Church may belong to
        /// Note that this does not include Archived GroupMembers
        /// </summary>
        public virtual ChurchGroup ChurchGroup { get; set; }

        #endregion
    }
}
