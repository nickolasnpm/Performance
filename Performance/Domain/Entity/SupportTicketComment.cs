using Performance.Domain;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Performance.Domain.Entity
{
    public class SupportTicketComment: BaseModel
    {
        [StringLength(2500)]
        public required string CommentText { get; set; }

        [StringLength(2500)]
        public required string RecommendedAction { get; set; }

        public long TicketId { get; set; }

        [JsonIgnore]
        public SupportTicket Ticket { get; set; } = null!;
    }
}
