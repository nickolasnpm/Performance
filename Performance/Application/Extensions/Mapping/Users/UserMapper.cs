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
        public static UserDTO EntityToDTO(User user) => new()
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
            Address = user.Address?.MapEntityToDTO(AddressMapper.ToDTO) ?? null,
            Roles = user.Roles?.MapEntityToDTO(RoleMapper.ToDTO).ToList() ?? null,
            BankAccount = user.BankAccount?.MapEntityToDTO(BankAccountMapper.ToDTO) ?? null,
            CreditCards = user.CreditCards?.MapEntityToDTO(CreditCardMapper.ToDTO) ?? null,
            Loans = user.Loans?.MapEntityToDTO(LoanMapper.ToDTO).ToList() ?? null,
            SupportTickets = user.SupportTickets?.MapEntityToDTO(SupportTicketMapper.ToDTO) ?? null
        };

        public static User AddRequestToEntity(AddUserRequestDTO request) => new()
        {
            Username = request.Username,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            PhoneNumber = request.PhoneNumber,
            ProfilePictureUrl = request.ProfilePictureUrl,
            CreatedAt = DateTimeOffset.Now,
            CreatedBy = "system",
            UpdatedAt = DateTimeOffset.Now,
            UpdatedBy = "system"
        };

        public static void UpdateRequestToEntity(UpdateUserRequestDTO request, User existingUser)
        {
            existingUser.FirstName = request.FirstName ?? existingUser.FirstName;
            existingUser.LastName = request.LastName ?? existingUser.LastName;
            existingUser.DateOfBirth = request.DateOfBirth ?? existingUser.DateOfBirth;
            existingUser.PhoneNumber = request.PhoneNumber ?? existingUser.PhoneNumber;
            existingUser.ProfilePictureUrl = request.ProfilePictureUrl ?? existingUser.ProfilePictureUrl;
            existingUser.UpdatedAt = DateTimeOffset.Now;
            existingUser.UpdatedBy = "system";
        }
    }
}