using Performance.Application.DTOs.SupportTicketComments;
using Performance.Application.Interface.Security;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping
{
    public static class SupportTicketCommentMapper
    {
        extension(SupportTicketComment comment)
        {
            public SupportTicketCommentDTO ToDTO(IIdHelper idHelper) => new(
                Id: idHelper.EncryptId(comment.Id),
                CommentText: comment.CommentText,
                RecommendedAction: comment.RecommendedAction
            );
        }
    }
}