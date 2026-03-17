using System.Diagnostics.CodeAnalysis;
using Performance.Domain.Entity;

namespace Performance.Application.DTOs.SupportTicketComments
{
    public class SupportTicketCommentDTO
    {
        public long Id { get; set; }
        public required string CommentText { get; set; }
        public required string RecommendedAction { get; set; }
        public long TicketId { get; set; }


        // only for mapping from SupportTicketComment entity to SupportTicketCommentDTO
        [SetsRequiredMembers]
        public SupportTicketCommentDTO(SupportTicketComment comment)
        {
            Id = comment.Id;
            CommentText = comment.CommentText;
            RecommendedAction = comment.RecommendedAction;
            TicketId = comment.TicketId;
        }
    }
}