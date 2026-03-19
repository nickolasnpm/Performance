using Performance.Application.DTOs.LoanRepayments;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping.LoanRepayments
{
    public static class LoanRepaymentMapper
    {
        public static LoanRepaymentDTO ToDTO(this LoanRepayment loanRepayment) => new()
        {
            Id = loanRepayment.Id,
            ScheduledDate = loanRepayment.ScheduledDate,
            ActualPaymentDate = loanRepayment.ActualPaymentDate,
            ScheduledAmount = loanRepayment.ScheduledAmount,
            PaidAmount = loanRepayment.PaidAmount,
            LoanId = loanRepayment.LoanId,
        };
    }
}