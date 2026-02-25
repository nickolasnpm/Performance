using Microsoft.EntityFrameworkCore;
using Performance.Domain;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Performance.Domain.Entity
{
    public class BankTransaction: BaseModel
    {
        [Precision(18, 2)]
        public decimal BaseAmount { get; set; } = decimal.Zero;

        [Precision(18, 2)]
        public decimal FeeAmount { get; set; } = decimal.Zero;

        [Precision(18, 2)]
        public decimal SettlementAmount { get; set; } = decimal.Zero;
        public int TransactionType { get; set; }
        public int Status { get; set; }

        [StringLength(200)]
        public required string MerchantName { get; set; }

        [StringLength(50)]
        public required string ReferenceNumber { get; set; }

        public long BankAccountId { get; set; }

        [JsonIgnore]
        public BankAccount BankAccount { get; set; } = null!;
    }
}
