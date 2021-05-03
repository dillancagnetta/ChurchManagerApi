﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChurchManager.Core.Shared
{
    public class GroupViewModel
    {
        public int Id { get; set; }
        public GroupTypeViewModel GroupType { get; set; }
        public int? ChurchId { get; set; }
        public int? ParentGroupId { get; set; }
        public string ParentGroupName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public bool? IsOnline { get; set; }

        public DateTime CreatedDate { get; set; }

        public IEnumerable<GroupViewModel> Groups { get; set; }
    }

    public record GroupTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GroupTerm { get; set; } 
        public string GroupMemberTerm { get; set; }
        public bool TakesAttendance { get; set; }
        public bool IsSystem { get; set; }
        public string IconCssClass { get; set; }
    }
}
