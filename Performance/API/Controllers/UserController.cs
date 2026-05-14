using Microsoft.AspNetCore.Mvc;
using Performance.Application.Interface.Services;
using Performance.Application.DTOs;
using Performance.Application.DTOs.Users;

namespace Performance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserServices userServices)
        : BaseController
    {
        private const string _bulkRoute = "bulk";

        [HttpGet]
        [ProducesResponseType(typeof(ListResponseDTO<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ListResponseDTO<UserDTO>>> GetPaginatedUsers([FromQuery] ListRequestDTO request)
        {
            var result = await userServices.GetPaginatedListAsync(request);
            return result.IsSuccess ? Ok(result.Data) : ToProblem(result.Error!);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDTO>> GetUserById([FromRoute] string id)
        {
            var result = await userServices.GetByIdAsync(id);
            return result.IsSuccess ? Ok(result.Data) : ToProblem(result.Error!);
        }

        [HttpPost(_bulkRoute)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> CreateUsers([FromBody] List<AddUserRequestDTO> requestDTOs)
        {
            var result = await userServices.CreateBulkUsers(requestDTOs);
            return result.IsSuccess ? Ok(result.Data) : ToProblem(result.Error!);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> CreateSingleUser([FromBody] AddUserRequestDTO requestDTO)
        {
            var result = await userServices.CreateUser(requestDTO);
            return result.IsSuccess ? Ok(result.Data) : ToProblem(result.Error!);
        }

        [HttpPut(_bulkRoute)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> UpdateUsers([FromBody] List<UpdateUserRequestDTO> requestDTOs)
        {
            var result = await userServices.UpdateBulkUsers(requestDTOs);
            return result.IsSuccess ? Ok(result.Data) : ToProblem(result.Error!);
        }

        [HttpDelete(_bulkRoute)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteUsers([FromBody] HashSet<string> ids)
        {
            var result = await userServices.DeleteBulkUsers(ids);
            return result.IsSuccess ? Ok(result.Data) : ToProblem(result.Error!);
        }
    }
}
