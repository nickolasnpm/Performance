using Performance.Application.DTOs.SupportTicketComments;

namespace Performance.Application.DTOs.SupportTickets
{
    public class SupportTicketDTO
    {
        public required string Id { get; set; }
        public required string Subject { get; set; }
        public required string Description { get; set; }
        public int Priority { get; set; }
        public bool IsResolved { get; set; }
        public required string UserId { get; set; }
        public List<SupportTicketCommentDTO>? Comments { get; set; }
    }
}