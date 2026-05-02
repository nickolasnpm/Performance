using Performance.Application.DTOs.Users;
using Performance.Domain.Entity;
using Performance.Application.Interface.Security;

namespace Performance.Application.Extensions.Mapping
{
    public static class UserMapper
    {
        extension(User user)
        {
            public UserDTO EntityToDTO(IIdHelper idHelper) => new(
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
                Address: user.Address?.ToDTO(idHelper),
                Roles: user.Roles?.Select(r => r.ToDTO(idHelper)).ToList(),
                BankAccount: user.BankAccount?.ToDTO(idHelper),
                CreditCards: user.CreditCards?.Select(c => c.ToDTO(idHelper)).ToList(),
                Loans: user.Loans?.Select(l => l.ToDTO(idHelper)).ToList(),
                SupportTickets: user.SupportTickets?.Select(s => s.ToDTO(idHelper)).ToList()
            );
        }

        extension(AddUserRequestDTO request)
        {
            public User AddRequestToEntity() => new()
            {
                Username = request.Username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                PhoneNumber = request.PhoneNumber,
                ProfilePictureUrl = request.ProfilePictureUrl
            };
        }

        extension(UpdateUserRequestDTO request)
        {
            public void UpdateRequestToEntity(User existingUser)
            {
                existingUser.FirstName = request.FirstName ?? existingUser.FirstName;
                existingUser.LastName = request.LastName ?? existingUser.LastName;
                existingUser.DateOfBirth = request.DateOfBirth ?? existingUser.DateOfBirth;
                existingUser.PhoneNumber = request.PhoneNumber ?? existingUser.PhoneNumber;
                existingUser.ProfilePictureUrl = request.ProfilePictureUrl ?? existingUser.ProfilePictureUrl;
            }
        }
    }
}