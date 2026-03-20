using Performance.Application.DTOs.Users;
using Performance.Application.Extensions.Mapping.Addresses;
using Performance.Application.Extensions.Mapping.BankAccounts;
using Performance.Application.Extensions.Mapping.CreditCards;
using Performance.Application.Extensions.Mapping.Roles;
using Performance.Application.Extensions.Mapping.Loans;
using Performance.Application.Extensions.Mapping.SupportTickets;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping.Users
{
    public static class UserMapper
    {
        public static UserDTO ToDTO(User user) => new()
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DateOfBirth = user.DateOfBirth,
            PhoneNumber = user.PhoneNumber,
            ProfilePictureUrl = user.ProfilePictureUrl,
            IsEmailVerified = user.IsEmailVerified,
            IsActive = user.IsActive,
            LastLoginAt = user.LastLoginAt,
            Address = user.Address?.MapToDTO(AddressMapper.ToDTO) ?? null,
            Roles = user.Roles?.MapToDTO(RoleMapper.ToDTO).ToList() ?? null,
            BankAccount = user.BankAccount?.MapToDTO(BankAccountMapper.ToDTO) ?? null,
            CreditCards = user.CreditCards?.MapToDTO(CreditCardMapper.ToDTO) ?? null,
            Loans = user.Loans?.MapToDTO(LoanMapper.ToDTO).ToList() ?? null,
            SupportTickets = user.SupportTickets?.MapToDTO(SupportTicketMapper.ToDTO) ?? null
        };
    }
}