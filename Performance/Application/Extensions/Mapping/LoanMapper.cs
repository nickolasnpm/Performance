using Performance.Application.DTOs.Loans;
using Performance.Application.Interface.Security;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping
{
    public static class LoanMapper
    {
        extension(Loan loan)
        {
            public LoanDTO ToDTO(IIdHelper idHelper) => new(
                Id: idHelper.EncryptId(loan.Id),
                LoanType: loan.LoanType,
                PrincipalAmount: loan.PrincipalAmount,
                InterestRate: loan.InterestRate,
                InterestAmount: loan.InterestAmount,
                TotalAmountToRepay: loan.TotalAmountToRepay,
                RemainingBalance: loan.RemainingBalance,
                TotalLoanTerms: loan.TotalLoanTerms,
                RemainingLoanTerms: loan.RemainingLoanTerms,
                MonthlyPaymentAmount: loan.MonthlyPaymentAmount,
                IsFullyPaid: loan.IsFullyPaid,
                Repayments: loan.Repayments?.Select(lr => lr.ToDTO(idHelper)).ToList()
            );
        }
    }
}