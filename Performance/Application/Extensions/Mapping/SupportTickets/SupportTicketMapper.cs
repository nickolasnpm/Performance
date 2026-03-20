using Performance.Application.DTOs.SupportTickets;
using Performance.Application.Extensions.Mapping.SupportTicketComments;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping.SupportTickets
{
    public static class SupportTicketMapper
    {
        public static SupportTicketDTO ToDTO(this SupportTicket supportTicket) => new()
        {
            Id = supportTicket.Id,
            UserId = supportTicket.UserId,
            Subject = supportTicket.Subject,
            Description = supportTicket.Description,
            Priority = supportTicket.Priority,
            IsResolved = supportTicket.IsResolved,
            Comments = supportTicket.Comments != null ? supportTicket.Comments.MapToDTO(SupportTicketCommentMapper.ToDTO) : null
        };
    }
}