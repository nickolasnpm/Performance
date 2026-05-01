namespace Performance.Application.DTOs.LoanRepayments
{
    public record LoanRepaymentDTO(
        string Id,
        DateTime ScheduledDate,
        DateTime? ActualPaymentDate,
        decimal ScheduledAmount,
        decimal PaidAmount);
}