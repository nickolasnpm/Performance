using Performance.Application.Common.Models;
using Performance.Application.DTOs.Users;
using Performance.Domain.Entity;

namespace Performance.Application.Interface.Services
{
    public interface IUserServices: IBaseServices<User, UserResponseDTO<UserDTO>, UserRequestDTO>
    {
        Task<Result<bool, List<AddErrorResponseDTO>>> CreateUsers(List<AddUserRequestDTO> requestDTOs);
        Task<Result<bool, List<long>>> UpdateUsers(List<UpdateUserRequestDTO> requestDTOs);
        Task<Result<bool, List<long>>> DeleteUsers(HashSet<long> ids);
    }
}
