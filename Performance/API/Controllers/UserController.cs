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
        [HttpGet("getusers")]
        [ProducesResponseType(typeof(UserResponseDTO<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserResponseDTO<UserDTO>>> GetPaginatedUsers([FromQuery] UserRequestDTO request)
        {
            return Ok(await userServices.GetPaginatedListAsync(request));
        }

        [HttpGet("getbyid")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDTO>> GetUserById([FromQuery] long Id)
        {
            var result = await userServices.GetByIdAsync(Id);

            if (result != null)
                return Ok(result);

            return NotFound();
        }

        [HttpPost("createusers")]
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

        [HttpPut("updateusers")]
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

        [HttpDelete("deleteusers")]
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
