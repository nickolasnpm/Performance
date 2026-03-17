using Performance.Application.DTOs.Users;
using Performance.Domain.Entity;

namespace Performance.Application.Interface.Services
{
    public interface IUserServices: IBaseServices<User, UserResponseDTO<UserDTO>, UserRequestDTO>
    {

    }
}
