using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Groups
{
    [Table("GroupType")]

    public record GroupType : IAggregateRoot<int>
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public required string Name { get; set; }
        [MaxLength(100)]
        public string? Description { get; set; }
        
        [MaxLength(50)]
        public string? Category { get; set; }
        
        [MaxLength(50)]
        public string GroupTerm { get; set; } = "Group";
        [MaxLength(50)]
        public string GroupMemberTerm { get; set; } = "Member";
        public bool TakesAttendance { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating if an attendance reminder should be sent to group leaders.
        /// </summary>
        public bool SendAttendanceReminder { get; set; } = false;
        
        public bool IsSystem { get; set; } = false;
        
        /// <summary>
        /// Gets or sets a flag indicating if this GroupType and its Groups are shown in Navigation.
        /// If false, this GroupType will be hidden navigation controls, such as TreeViews and Menus
        /// </summary>
        public bool ShowInNavigation { get; set; } = true;

        public string IconCssClass { get; set; } = "Group";

        /// <summary>
        /// Gets or sets a value indicating if group type allows any child group type.
        /// </summary>
        public bool AllowAnyChildGroupType { get; set; } = true;
        
        public ICollection<int> AllowedChildGroupTypesIds { get; set; } = new List<int>();
    }
}
