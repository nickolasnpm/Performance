using Performance.Application.DTOs.SupportTicketComments;
using Performance.Application.Interface.Hashing;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping.SupportTicketComments
{
    public static class SupportTicketCommentMapper
    {
        public static SupportTicketCommentDTO ToDTO(SupportTicketComment comment, IIdHelper idHelper) => new()
        {
            Id = idHelper.EncryptId(comment.Id),
            CommentText = comment.CommentText,
            RecommendedAction = comment.RecommendedAction
        };
    }
}