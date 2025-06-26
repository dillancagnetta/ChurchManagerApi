using System.ComponentModel.DataAnnotations.Schema;
using ChurchManager.Domain.Features.People;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.Communications
{
    [Table("PushDevice", Schema = "Communications")]
    public record PushDevice : IAggregateRoot<int>
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Endpoint { get; set; }
        public required string P256DH { get; set; }
        public required string Auth { get; set; }
        public required string UniqueIdentification { get; set; }

        public int PersonId { get; set; }

        #region Navigation

        public virtual Person? Person { get; set; }

        #endregion
    }
}
