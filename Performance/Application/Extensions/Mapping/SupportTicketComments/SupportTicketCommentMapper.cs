using Performance.Application.Common.Prefix;
using Performance.Application.DTOs.SupportTicketComments;
using Performance.Application.Interface.Hashing;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping.SupportTicketComments
{
    public static class SupportTicketCommentMapper
    {
        public static SupportTicketCommentDTO ToDTO(SupportTicketComment comment, IIdHelper idHelper) => new()
        {
            Id = idHelper.EncodeId(comment.Id, IdPrefix.SupportTicketComment),
            CommentText = comment.CommentText,
            RecommendedAction = comment.RecommendedAction,
            TicketId = idHelper.EncodeId(comment.TicketId, IdPrefix.SupportTicket),
        };
    }
}