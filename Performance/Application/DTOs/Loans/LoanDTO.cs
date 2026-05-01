using Performance.Application.DTOs.LoanRepayments;

namespace Performance.Application.DTOs.Loans
{
    public record LoanDTO(
        string Id,
        string LoanType,
        decimal PrincipalAmount,
        decimal InterestRate,
        decimal InterestAmount,
        decimal TotalAmountToRepay,
        decimal RemainingBalance,
        int TotalLoanTerms,
        int RemainingLoanTerms,
        decimal MonthlyPaymentAmount,
        bool IsFullyPaid,
        List<LoanRepaymentDTO>? Repayments);
}