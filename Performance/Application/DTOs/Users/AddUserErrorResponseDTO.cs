namespace Performance.Application.DTOs.Users
{
    public record AddUserErrorResponseDTO(
        string Username,
        bool? IsUsernameDuplicated,
        bool? IsUsernameExist,
        string Email,
        bool? IsEmailDuplicated,
        bool? IsEmailExist
    );
}