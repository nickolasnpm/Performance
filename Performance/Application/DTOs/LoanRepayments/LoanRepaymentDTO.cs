using System.Diagnostics.CodeAnalysis;
using Performance.Domain.Entity;

namespace Performance.Application.DTOs.LoanRepayments
{
    public class LoanRepaymentDTO
    {
        public long Id { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime? ActualPaymentDate { get; set; }
        public decimal ScheduledAmount { get; set; } = decimal.Zero;
        public decimal PaidAmount { get; set; } = decimal.Zero;
        public long LoanId { get; set; }


        // only for mapping from LoanRepayment entity to LoanRepaymentDTO
        [SetsRequiredMembers]
        public LoanRepaymentDTO(LoanRepayment repayment)
        {
            Id = repayment.Id;
            ScheduledDate = repayment.ScheduledDate;
            ActualPaymentDate = repayment.ActualPaymentDate;
            ScheduledAmount = repayment.ScheduledAmount;
            PaidAmount = repayment.PaidAmount;
            LoanId = repayment.LoanId;
        }
    }
}