using Performance.Application.DTOs.LoanRepayments;

namespace Performance.Application.DTOs.Loans
{
    public class LoanDTO
    {
        public required string Id { get; set; }
        public required string LoanType { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; } 
        public decimal InterestAmount { get; set; } 
        public decimal TotalAmountToRepay { get; set; }
        public decimal RemainingBalance { get; set; }
        public int TotalLoanTerms { get; set; }
        public int RemainingLoanTerms { get; set; }
        public decimal MonthlyPaymentAmount { get; set; }
        public bool IsFullyPaid { get; set; } = false;
        public required string UserId { get; set; }
        public List<LoanRepaymentDTO>? Repayments { get; set; }
    }
}