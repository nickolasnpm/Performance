using System.ComponentModel.DataAnnotations;

namespace Performance.Application.DTOs.Users
{
    public class AddUserRequestDTO
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
    }
}
