using Performance.Application.DTOs.LoanRepayments;
using Performance.Application.Interface.Hashing;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping.LoanRepayments
{
    public static class LoanRepaymentMapper
    {
        public static LoanRepaymentDTO ToDTO(LoanRepayment loanRepayment, IIdHelper idHelper) => new()
        {
            Id = idHelper.EncryptId(loanRepayment.Id),
            ScheduledDate = loanRepayment.ScheduledDate,
            ActualPaymentDate = loanRepayment.ActualPaymentDate,
            ScheduledAmount = loanRepayment.ScheduledAmount,
            PaidAmount = loanRepayment.PaidAmount
        };
    }
}