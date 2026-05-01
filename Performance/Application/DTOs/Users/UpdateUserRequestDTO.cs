using System.ComponentModel.DataAnnotations;

namespace Performance.Application.DTOs.Users
{
    public record UpdateUserRequestDTO(
        string Id,
        [StringLength(100)] string? FirstName,
        [StringLength(100)] string? LastName,
        DateOnly? DateOfBirth,
        [Phone] string? PhoneNumber,
        [StringLength(250)] string? ProfilePictureUrl
    );
}