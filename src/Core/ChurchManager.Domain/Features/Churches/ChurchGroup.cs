﻿using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Churches
{
    [Table("ChurchGroup")]

    public class ChurchGroup : Entity<int>, IAggregateRoot<int>
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Id of the <see cref="Person"/> that is the leader of the campus.
        /// </summary>
        public int? LeaderPersonId { get; set; }

        #region Navigation


        /// <summary>
        /// Gets or sets a collection containing the <see cref="Church">Churches</see> who are associated with the ChurchGroup.
        /// Note that this does not include Archived GroupMembers
        /// </summary>
        public virtual ICollection<Church> Churches { get; set; } = new Collection<Church>();

        public virtual Person LeaderPerson { get; set; }
        
        #endregion
    }
}
