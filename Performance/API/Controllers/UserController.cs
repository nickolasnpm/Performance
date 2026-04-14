using Microsoft.AspNetCore.Mvc;
using Performance.Application.Interface.Services;
using Performance.Application.DTOs.Users;
using Performance.Application.Common.Models;

namespace Performance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserServices userServices)
        : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ListResponseDTO<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ListResponseDTO<UserDTO>>> GetPaginatedUsers([FromQuery] ListRequestDTO request)
        {
            var result = await userServices.GetPaginatedListAsync(request);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDTO>> GetUserById([FromRoute] long Id)
        {
            var result = await userServices.GetByIdAsync(Id);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.Error);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResultError<List<AddErrorResponseDTO>>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> CreateUsers([FromBody] List<AddUserRequestDTO> requestDTOs)
        {
            var result = await userServices.CreateUsers(requestDTOs);
            return result.IsSuccess ? Ok(result.Data) : result.Error?.Code switch
            {
                StatusCodes.Status400BadRequest => BadRequest(result.Error),
                StatusCodes.Status409Conflict => Conflict(result.Error),
                _ => throw new Exception()
            };
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResultError<List<long>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> UpdateUsers([FromBody] List<UpdateUserRequestDTO> requestDTOs)
        {
            var result = await userServices.UpdateUsers(requestDTOs);
            return result.IsSuccess ? Ok(result.Data) : result.Error?.Code switch
            {
                StatusCodes.Status400BadRequest => BadRequest(result.Error),
                StatusCodes.Status404NotFound => NotFound(result.Error),
                _ => throw new Exception()
            };
        }

        [HttpDelete]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResultError<List<long>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteUsers([FromBody] HashSet<long> ids)
        {
            var result = await userServices.DeleteUsers(ids);
            return result.IsSuccess ? Ok(result.Data) : result.Error?.Code switch
            {
                StatusCodes.Status400BadRequest => BadRequest(result.Error),
                StatusCodes.Status404NotFound => NotFound(result.Error),
                _ => throw new Exception()
            };
        }
    }
}
