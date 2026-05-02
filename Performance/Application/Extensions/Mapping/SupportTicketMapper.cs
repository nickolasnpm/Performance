using Performance.Application.DTOs.SupportTickets;
using Performance.Application.Interface.Security;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping
{
    public static class SupportTicketMapper
    {
        extension(SupportTicket supportTicket)
        {
            public SupportTicketDTO ToDTO(IIdHelper idHelper) => new(
                Id: idHelper.EncryptId(supportTicket.Id),
                Subject: supportTicket.Subject,
                Description: supportTicket.Description,
                Priority: supportTicket.Priority,
                IsResolved: supportTicket.IsResolved,
                Comments: supportTicket.Comments?.Select(cmt => cmt.ToDTO(idHelper)).ToList()
            );
        }
    }
}