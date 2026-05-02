using Performance.Application.DTOs.LoanRepayments;
using Performance.Application.Interface.Security;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping
{
    public static class LoanRepaymentMapper
    {
        extension(LoanRepayment loanRepayment)
        {
            public LoanRepaymentDTO ToDTO(IIdHelper idHelper) => new(
                Id: idHelper.EncryptId(loanRepayment.Id),
                ScheduledDate: loanRepayment.ScheduledDate,
                ActualPaymentDate: loanRepayment.ActualPaymentDate,
                ScheduledAmount: loanRepayment.ScheduledAmount,
                PaidAmount: loanRepayment.PaidAmount
            );
        }
    }
}