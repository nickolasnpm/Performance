using System.Diagnostics.CodeAnalysis;
using Performance.Application.DTOs.Addresses;
using Performance.Application.DTOs.BankAccounts;
using Performance.Application.DTOs.CreditCards;
using Performance.Application.DTOs.Loans;
using Performance.Application.DTOs.Roles;
using Performance.Application.DTOs.SupportTickets;
using Performance.Domain.Entity;

namespace Performance.Application.DTOs.Users
{
    public class UserDTO
    {
        public long Id { get; set; }
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
        public List<RoleDTO> Roles { get; set; } = new List<RoleDTO>();
        public BankAccountDTO? BankAccount { get; set; }
        public List<CreditCardDTO> CreditCards { get; set; } = new List<CreditCardDTO>();
        public List<LoanDTO> Loans { get; set; } = new List<LoanDTO>();
        public List<SupportTicketDTO> SupportTickets { get; set; } = new List<SupportTicketDTO>();


        // only for mapping from User entity to UserDTO
        [SetsRequiredMembers]
        public UserDTO(User user)
        {
            Id = user.Id;
            Username = user.Username;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            DateOfBirth = user.DateOfBirth;
            PhoneNumber = user.PhoneNumber;
            ProfilePictureUrl = user.ProfilePictureUrl;
            IsEmailVerified = user.IsEmailVerified;
            IsActive = user.IsActive;
            LastLoginAt = user.LastLoginAt;

            if (user.Address != null)
                Address = new AddressDTO(user.Address);
            if (user.Roles != null)
                Roles = user.Roles.Select(r => new RoleDTO(r)).ToList();
            if (user.BankAccount != null)
                BankAccount = new BankAccountDTO(user.BankAccount);
            if (user.CreditCards != null)
                CreditCards = user.CreditCards.Select(cc => new CreditCardDTO(cc)).ToList();
            if (user.Loans != null)
                Loans = user.Loans.Select(loan => new LoanDTO(loan)).ToList();
            if (user.SupportTickets != null)
                SupportTickets = user.SupportTickets.Select(st => new SupportTicketDTO(st)).ToList();
        }
    }
}