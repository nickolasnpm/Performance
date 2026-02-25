using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Performance.Domain.Entity
{
    [Index(nameof(Name), IsUnique = true)]
    public class Role: BaseModel
    {
        [StringLength(50)]
        public required string Name { get; set; }

        [JsonIgnore]
        public List<User> Users { get; set; } = new List<User>();
    }
}
