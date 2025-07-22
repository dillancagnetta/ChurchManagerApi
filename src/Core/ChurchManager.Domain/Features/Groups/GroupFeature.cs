using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Domain.Features.Groups
{
    [Table("GroupFeature", Schema = "Groups" )]
    public class GroupFeature : Entity<int>
    {
        public required string Name { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Group> Groups { get; set; } = [];
    }
}
