using Performance.Application.Common.Prefix;
using Performance.Application.DTOs.SupportTickets;
using Performance.Application.Extensions.Mapping.SupportTicketComments;
using Performance.Application.Interface.Hashing;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping.SupportTickets
{
    public static class SupportTicketMapper
    {
        public static SupportTicketDTO ToDTO(SupportTicket supportTicket, IIdHelper idHelper) => new()
        {
            Id = idHelper.EncodeId(supportTicket.Id, IdPrefix.SupportTicket),
            Subject = supportTicket.Subject,
            Description = supportTicket.Description,
            Priority = supportTicket.Priority,
            IsResolved = supportTicket.IsResolved,
            UserId = idHelper.EncodeId(supportTicket.UserId, IdPrefix.User),
            Comments = supportTicket.Comments?.Map(cmt => SupportTicketCommentMapper.ToDTO(cmt, idHelper)) ?? null
        };
    }
}