using System.ComponentModel.DataAnnotations;

namespace Performance.Application.DTOs.Users
{
    public class UpdateUserRequestDTO
    {
        public long Id { get; set; }

        [StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? LastName { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        [StringLength(250)]
        public string? ProfilePictureUrl { get; set; }
    }
}
