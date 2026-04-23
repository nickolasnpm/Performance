namespace Performance.Application.DTOs.LoanRepayments
{
    public class LoanRepaymentDTO
    {
        public required string Id { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime? ActualPaymentDate { get; set; }
        public decimal ScheduledAmount { get; set; }
        public decimal PaidAmount { get; set; }
    }
}