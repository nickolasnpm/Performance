using Performance.Application.DTOs.SupportTicketComments;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping.SupportTicketComments
{
    public static class SupportTicketCommentMapper
    {
        public static SupportTicketCommentDTO ToDTO(SupportTicketComment comment) => new()
        {
            Id = comment.Id,
            CommentText = comment.CommentText,
            RecommendedAction = comment.RecommendedAction,
            TicketId = comment.TicketId,
        };
    }
}