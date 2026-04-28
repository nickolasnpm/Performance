using Performance.Application.Common.Models;
using Performance.Application.DTOs.Users;

namespace Performance.Application.Interface.Services
{
    public interface IUserServices: IBaseServices<UserDTO, ListRequestDTO>
    {
        Task<Result<bool, ResultError>> CreateUsers(List<AddUserRequestDTO> requestDTOs);
        Task<Result<bool, ResultError>> UpdateUsers(List<UpdateUserRequestDTO> requestDTOs);
        Task<Result<bool, ResultError>> DeleteUsers(HashSet<long> ids);
    }
}
