using System.ComponentModel.DataAnnotations;

namespace Performance.Domain.Entity
{
    public class BaseModel
    {
        public long Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        [StringLength(50)]
        public required string CreatedBy { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        [StringLength(50)]
        public required string UpdatedBy { get; set; }
    }
}
