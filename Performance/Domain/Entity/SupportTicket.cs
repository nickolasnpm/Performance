using Performance.Domain;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Performance.Domain.Entity
{
    public class SupportTicket: BaseModel
    {
        [StringLength(500)]
        public required string Subject { get; set; }

        [StringLength(2500)]
        public required string Description { get; set; } 
        public int Priority { get; set; }
        public bool IsResolved { get; set; }

        public long UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; } = null!;

        public List<SupportTicketComment> Comments { get; set; } = new List<SupportTicketComment>();
    }
}
