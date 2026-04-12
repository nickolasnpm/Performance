namespace Performance.Application.DTOs.Users
{
    public class AddErrorResponseDTO
    {
        public required string Username { get; set; }
        public bool IsUsernameExist { get; set; }
        public required string Email { get; set; }
        public bool IsEmailExist { get; set; }
    }
}
