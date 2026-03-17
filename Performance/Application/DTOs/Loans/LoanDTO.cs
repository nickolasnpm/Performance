using System.Diagnostics.CodeAnalysis;
using Performance.Application.DTOs.LoanRepayments;
using Performance.Domain.Entity;

namespace Performance.Application.DTOs.Loans
{
    public class LoanDTO
    {
        public long Id { get; set; }
        public required string LoanType { get; set; }
        public decimal PrincipalAmount { get; set; } = decimal.Zero;
        public decimal InterestRate { get; set; } = decimal.Zero;
        public decimal InterestAmount { get; set; } = decimal.Zero;
        public decimal TotalAmountToRepay { get; set; } = decimal.Zero;
        public decimal RemainingBalance { get; set; } = decimal.Zero;
        public int TotalLoanTerms { get; set; }
        public int RemainingLoanTerms { get; set; }
        public decimal MonthlyPaymentAmount { get; set; } = decimal.Zero;
        public bool IsFullyPaid { get; set; } = false;
        public long UserId { get; set; }
        public List<LoanRepaymentDTO> Repayments { get; set; } = new List<LoanRepaymentDTO>();


        // only for mapping from Loan entity to LoanDTO
        [SetsRequiredMembers]
        public LoanDTO(Loan loan)
        {
            Id = loan.Id;
            LoanType = loan.LoanType;
            PrincipalAmount = loan.PrincipalAmount;
            InterestRate = loan.InterestRate;
            InterestAmount = loan.InterestAmount;
            TotalAmountToRepay = loan.TotalAmountToRepay;
            RemainingBalance = loan.RemainingBalance;
            TotalLoanTerms = loan.TotalLoanTerms;
            RemainingLoanTerms = loan.RemainingLoanTerms;
            MonthlyPaymentAmount = loan.MonthlyPaymentAmount;
            IsFullyPaid = loan.IsFullyPaid;
            UserId = loan.UserId;
            if (loan.Repayments != null)
                Repayments = loan.Repayments.Select(r => new LoanRepaymentDTO(r)).ToList();
        }
    }
}