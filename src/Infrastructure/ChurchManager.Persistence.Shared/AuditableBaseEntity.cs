using System.ComponentModel.DataAnnotations;

namespace ChurchManager.Persistence.Shared
{
    public abstract class AuditableEntity<TPrimaryKey> : Entity<TPrimaryKey>
    {
        [MaxLength(50)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        [MaxLength(50)]
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
