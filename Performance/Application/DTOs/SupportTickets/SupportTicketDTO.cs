using Performance.Application.DTOs.SupportTicketComments;

namespace Performance.Application.DTOs.SupportTickets
{
    public record SupportTicketDTO(
        string Id,
        string Subject,
        string Description,
        int Priority,
        bool IsResolved,
        List<SupportTicketCommentDTO>? Comments
    );
}