using Microsoft.EntityFrameworkCore;
using Performance.Domain;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Performance.Domain.Entity
{
    [Index(nameof(AccountNumber), IsUnique = true)]
    public class BankAccount : BaseModel
    {
        [StringLength(20)]
        public required string AccountNumber { get; set; }

        [Precision(18, 2)]
        public decimal CurrentBalance { get; set; } = decimal.Zero;

        [Precision(18, 2)]
        public decimal AvailableBalance { get; set; } = decimal.Zero;

        public long UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; } = null!;

        public List<BankTransaction> Transactions { get; set; } = new List<BankTransaction>();
    }
}
