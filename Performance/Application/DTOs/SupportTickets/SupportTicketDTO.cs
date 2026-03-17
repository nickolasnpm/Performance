using System.Diagnostics.CodeAnalysis;
using Performance.Application.DTOs.SupportTicketComments;
using Performance.Domain.Entity;

namespace Performance.Application.DTOs.SupportTickets
{
    public class SupportTicketDTO
    {
        public long Id { get; set; }
        public required string Subject { get; set; }
        public required string Description { get; set; }
        public int Priority { get; set; }
        public bool IsResolved { get; set; }
        public long UserId { get; set; }
        public List<SupportTicketCommentDTO> Comments { get; set; } = new List<SupportTicketCommentDTO>();


        // only for mapping from SupportTicket entity to SupportTicketDTO
        [SetsRequiredMembers]
        public SupportTicketDTO(SupportTicket supportTicket)
        {
            Id = supportTicket.Id;
            Subject = supportTicket.Subject;
            Description = supportTicket.Description;
            Priority = supportTicket.Priority;
            IsResolved = supportTicket.IsResolved;
            UserId = supportTicket.UserId;
        }
    }
}