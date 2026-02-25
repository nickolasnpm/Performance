using Microsoft.EntityFrameworkCore;
using Performance.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Performance.Domain.Entity
{
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User: BaseModel
    {
        [StringLength(50)]
        public required string Username { get; set; }

        [StringLength(50)]
        public required string Email { get; set; }

        [StringLength(100)]
        public required string FirstName { get; set; }

        [StringLength(100)]
        public required string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }

        [Phone]
        public required string PhoneNumber { get; set; }

        [StringLength(250)]
        public string? ProfilePictureUrl { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLoginAt { get; set; }

        public Address? Address { get; set; }
        public List<Role> Roles { get; set; } = new List<Role>();
        public BankAccount? BankAccount { get; set; }
        public List<CreditCard> CreditCards { get; set; } = new List<CreditCard>();
        public List<Loan> Loans { get; set; } = new List<Loan>();
        public List<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();
    }
}
