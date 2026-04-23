using System.Diagnostics.CodeAnalysis;
using Performance.Domain.Entity;

namespace Performance.Application.DTOs.SupportTicketComments
{
    public class SupportTicketCommentDTO
    {
        public required string Id { get; set; }
        public required string CommentText { get; set; }
        public required string RecommendedAction { get; set; }
    }
}