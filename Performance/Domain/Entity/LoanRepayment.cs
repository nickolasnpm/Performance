using Microsoft.EntityFrameworkCore;
using Performance.Domain;
using System.Text.Json.Serialization;

namespace Performance.Domain.Entity
{
    public class LoanRepayment: BaseModel
    {
        public DateTime ScheduledDate { get; set; }
        public DateTime? ActualPaymentDate { get; set; }

        [Precision(18, 2)]
        public decimal ScheduledAmount { get; set; } = decimal.Zero;

        [Precision(18, 2)] 
        public decimal PaidAmount { get; set; } = decimal.Zero;

        public long LoanId { get; set; }

        [JsonIgnore]
        public Loan Loan { get; set; } = null!;
    }
}
