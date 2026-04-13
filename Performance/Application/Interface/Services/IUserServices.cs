using Performance.Application.Common.Models;
using Performance.Application.DTOs.Users;
using Performance.Domain.Entity;

namespace Performance.Application.Interface.Services
{
    public interface IUserServices: IBaseServices<User, UserResponseDTO<UserDTO>, UserRequestDTO>
    {
        Task<Result<bool, ResultError>> CreateUsers(List<AddUserRequestDTO> requestDTOs);
        Task<Result<bool, ResultError>> UpdateUsers(List<UpdateUserRequestDTO> requestDTOs);
        Task<Result<bool, ResultError>> DeleteUsers(HashSet<long> ids);
    }
}
