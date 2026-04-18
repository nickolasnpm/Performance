using Performance.Application.DTOs.Addresses;
using Performance.Application.DTOs.BankAccounts;
using Performance.Application.DTOs.CreditCards;
using Performance.Application.DTOs.Loans;
using Performance.Application.DTOs.Roles;
using Performance.Application.DTOs.SupportTickets;

namespace Performance.Application.DTOs.Users
{
    public class UserDTO
    {
        public required string Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public required string PhoneNumber { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public AddressDTO? Address { get; set; }
        public List<RoleDTO>? Roles { get; set; }
        public BankAccountDTO? BankAccount { get; set; }
        public List<CreditCardDTO>? CreditCards { get; set; }
        public List<LoanDTO>? Loans { get; set; }
        public List<SupportTicketDTO>? SupportTickets { get; set; }
    }
}