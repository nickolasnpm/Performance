using System.ComponentModel.DataAnnotations;
using Performance.Application.DTOs.Addresses;

namespace Performance.Application.DTOs.Users
{
    public record AddUserRequestDTO(
        [StringLength(50)] string Username,
        [StringLength(50)] string Email,
        [StringLength(100)] string FirstName,
        [StringLength(100)] string LastName,
        DateOnly DateOfBirth,
        [Phone] string PhoneNumber,
        [StringLength(250)] string? ProfilePictureUrl,
        AddAddressRequestDTO? Address = null
    );
}
