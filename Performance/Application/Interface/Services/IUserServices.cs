using Performance.Application.Common.Models;
using Performance.Application.DTOs;
using Performance.Application.DTOs.Users;

namespace Performance.Application.Interface.Services
{
    public interface IUserServices: IBaseServices<UserDTO, ListRequestDTO>
    {
        Task<Result<bool, ResultError>> CreateBulkUsers(List<AddUserRequestDTO> requestDTOs);
        Task<Result<bool, ResultError>> CreateUser(AddUserRequestDTO requestDTO);
        Task<Result<bool, ResultError>> UpdateBulkUsers(List<UpdateUserRequestDTO> requestDTOs);
        Task<Result<bool, ResultError>> DeleteBulkUsers(HashSet<string> ids);
    }
}
