namespace Performance.Application.DTOs.SupportTicketComments
{
    public record SupportTicketCommentDTO(
        string Id,
        string CommentText,
        string RecommendedAction
    );
}