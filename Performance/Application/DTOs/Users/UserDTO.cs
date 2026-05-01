using Performance.Application.DTOs.Addresses;
using Performance.Application.DTOs.BankAccounts;
using Performance.Application.DTOs.CreditCards;
using Performance.Application.DTOs.Loans;
using Performance.Application.DTOs.Roles;
using Performance.Application.DTOs.SupportTickets;

namespace Performance.Application.DTOs.Users
{
    public record UserDTO(
        string? Id,
        string Username,
        string Email,
        string FirstName,
        string LastName,
        DateOnly DateOfBirth,
        string PhoneNumber,
        string? ProfilePictureUrl,
        bool IsEmailVerified,
        bool IsActive,
        DateTime? LastLoginAt,
        AddressDTO? Address,
        List<RoleDTO>? Roles,
        BankAccountDTO? BankAccount,
        List<CreditCardDTO>? CreditCards,
        List<LoanDTO>? Loans,
        List<SupportTicketDTO>? SupportTickets
    );
}