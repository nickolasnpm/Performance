using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Performance.Domain.Entity
{
    public class Address: BaseModel
    {
        [StringLength(250)]
        public required string AddressLine { get; set; }

        [StringLength(100)]
        public required string City { get; set; }

        [StringLength(100)]
        public required string State { get; set; }

        [StringLength(10)]
        public required string PostalCode { get; set; }

        [StringLength(100)]
        public required string Country { get; set; }

        public required long UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
    }
}
