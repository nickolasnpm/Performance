using Microsoft.AspNetCore.Mvc;
using Performance.Application.Interface.Services;
using Performance.Application.DTOs.Users;
using Performance.Application.Common.Models;

namespace Performance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserServices userServices)
        : BaseController
    {
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

        [HttpPost]
        [ProducesResponseType(typeof(CreateResultDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateResultDTO>> CreateUsers([FromBody] List<AddUserRequestDTO> requestDTOs)
        {
            var result = await userServices.CreateUsers(requestDTOs);
            return result.IsSuccess ? StatusCode(201, new CreateResultDTO { IsCreated = result.Data }) : ToProblem(result.Error!);
        }

        [HttpPut]
        [ProducesResponseType(typeof(UpdateResultDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UpdateResultDTO>> UpdateUsers([FromBody] List<UpdateUserRequestDTO> requestDTOs)
        {
            var result = await userServices.UpdateUsers(requestDTOs);
            return result.IsSuccess ? Ok(new UpdateResultDTO { IsUpdated = result.Data }) : ToProblem(result.Error!);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(DeleteResultDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DeleteResultDTO>> DeleteUsers([FromBody] HashSet<string> ids)
        {
            var result = await userServices.DeleteUsers(ids);
            return result.IsSuccess ? Ok(new DeleteResultDTO { IsDeleted = result.Data }) : ToProblem(result.Error!);
        }
    }
}
