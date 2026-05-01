namespace Performance.Application.DTOs.Users
{
    public record AddErrorResponseDTO(
        string Username,
        bool IsUsernameExist,
        string Email,
        bool IsEmailExist
    );
}