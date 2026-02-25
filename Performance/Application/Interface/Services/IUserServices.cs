using Performance.Application.DTO;
using Performance.Domain.Entity;

namespace Performance.Application.Interface.Services
{
    public interface IUserServices: IBaseServices<User, UserResponseDTO<User>, UserRequestDTO>
    {

    }
}
