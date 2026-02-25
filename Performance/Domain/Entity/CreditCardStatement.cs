using Microsoft.EntityFrameworkCore;
using Performance.Domain;
using System.Text.Json.Serialization;

namespace Performance.Domain.Entity
{
    public class CreditCardStatement : BaseModel
    {
        public DateTime StatementDate { get; set; }
        public DateTime DueDate { get; set; }

        [Precision(18, 2)]
        public decimal StatementBalance { get; set; } = decimal.Zero;

        [Precision(18, 2)]
        public decimal MinimumPayment { get; set; } = decimal.Zero;

        [Precision(18, 2)]
        public decimal PaymentsReceived { get; set; } = decimal.Zero;

        [Precision(18, 2)] 
        public decimal InterestCharged { get; set; } = decimal.Zero;

        [Precision(18, 2)] 
        public decimal AvailableCredit { get; set; } = decimal.Zero;

        public long CreditCardId { get; set; }

        [JsonIgnore]
        public CreditCard CreditCard { get; set; } = null!;
    }
}
