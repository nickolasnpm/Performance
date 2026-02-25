using Microsoft.EntityFrameworkCore;
using Performance.Domain;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Performance.Domain.Entity
{
    [Index(nameof(CardNumber), IsUnique = true)]
    public class CreditCard : BaseModel
    {
        [StringLength(50)]
        public required string CardNumber { get; set; }

        [StringLength(200)]
        public required string CardHolderName { get; set; }

        [StringLength(50)]
        public required string CardProvider { get; set; }

        [StringLength(50)]
        public required string Bank { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public bool IsDefault { get; set; }

        [Precision(18, 2)]
        public decimal CreditLimit { get; set; } = decimal.Zero;

        public long UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; } = null!;

        public List<CreditCardStatement> Statements { get; set; } = new List<CreditCardStatement>();
    }
}
