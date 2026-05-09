using System.ComponentModel.DataAnnotations;

namespace Performance.Domain.Entity
{
    public class AuditableEntity: BaseEntity
    {
        public DateTimeOffset CreatedAt { get; set; }

        [StringLength(50)]
        public string? CreatedBy { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        [StringLength(50)]
        public string? UpdatedBy { get; set; }
    }
}
