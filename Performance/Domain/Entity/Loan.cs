using Microsoft.EntityFrameworkCore;
using Performance.Domain;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Performance.Domain.Entity
{
    public class Loan : BaseModel
    {
        [StringLength(50)]
        public required string LoanType { get; set; }

        [Precision(18, 2)] 
        public decimal PrincipalAmount { get; set; } = decimal.Zero;

        [Precision(18, 2)] 
        public decimal InterestRate { get; set; } = decimal.Zero;

        [Precision(18, 2)]
        public decimal InterestAmount { get; set; } = decimal.Zero;

        [Precision(18, 2)] 
        public decimal TotalAmountToRepay { get; set; } = decimal.Zero;

        [Precision(18, 2)] 
        public decimal RemainingBalance { get; set; } = decimal.Zero;
        public int TotalLoanTerms { get; set; }
        public int RemainingLoanTerms { get; set; }

        [Precision(18, 2)]
        public decimal MonthlyPaymentAmount {  get; set; }  = decimal.Zero;
        public bool IsFullyPaid { get; set; } = false;

        public long UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; } = null!;

        public List<LoanRepayment> Repayments { get; set; } = new List<LoanRepayment>();
    }
}
