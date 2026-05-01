using Performance.Application.DTOs.Users;
using Performance.Application.Extensions.Mapping.Addresses;
using Performance.Application.Extensions.Mapping.BankAccounts;
using Performance.Application.Extensions.Mapping.CreditCards;
using Performance.Application.Extensions.Mapping.Roles;
using Performance.Application.Extensions.Mapping.Loans;
using Performance.Application.Extensions.Mapping.SupportTickets;
using Performance.Domain.Entity;
using Performance.Application.Interface.Security;

namespace Performance.Application.Extensions.Mapping.Users
{
    public static class UserMapper
    {
        public static UserDTO EntityToDTO(User user, IIdHelper idHelper) => new(
            Id: idHelper.EncryptId(user.Id),
            Username: user.Username,
            Email: user.Email,
            FirstName: user.FirstName,
            LastName: user.LastName,
            DateOfBirth: user.DateOfBirth,
            PhoneNumber: user.PhoneNumber,
            ProfilePictureUrl: user.ProfilePictureUrl,
            IsEmailVerified: user.IsEmailVerified,
            IsActive: user.IsActive,
            LastLoginAt: user.LastLoginAt,
            Address: user.Address?.Map(a => AddressMapper.ToDTO(a, idHelper)) ?? null,
            Roles: user.Roles?.Map(r => RoleMapper.ToDTO(r, idHelper)).ToList() ?? null,
            BankAccount: user.BankAccount?.Map(b => BankAccountMapper.ToDTO(b, idHelper)) ?? null,
            CreditCards: user.CreditCards?.Map(c => CreditCardMapper.ToDTO(c, idHelper)) ?? null,
            Loans: user.Loans?.Map(l => LoanMapper.ToDTO(l, idHelper)).ToList() ?? null,
            SupportTickets: user.SupportTickets?.Map(s => SupportTicketMapper.ToDTO(s, idHelper)) ?? null
        );

        public static User AddRequestToEntity(AddUserRequestDTO request) => new()
        {
            Username = request.Username,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            PhoneNumber = request.PhoneNumber,
            ProfilePictureUrl = request.ProfilePictureUrl
        };

        public static void UpdateRequestToEntity(User existingUser, UpdateUserRequestDTO request)
        {
            existingUser.FirstName = request.FirstName ?? existingUser.FirstName;
            existingUser.LastName = request.LastName ?? existingUser.LastName;
            existingUser.DateOfBirth = request.DateOfBirth ?? existingUser.DateOfBirth;
            existingUser.PhoneNumber = request.PhoneNumber ?? existingUser.PhoneNumber;
            existingUser.ProfilePictureUrl = request.ProfilePictureUrl ?? existingUser.ProfilePictureUrl;
        }
    }
}