using Microsoft.AspNetCore.Mvc;
using Performance.API.Exceptions;
using Performance.Application.Interface.Services;
using Performance.Application.DTOs.Users;

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

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] UserRequestDTO request)
        {
            if (request.PaginationType != (Application.Common.Enums.PaginationType)7)
                throw new NotSupportedException("told you so");

            return Ok();
        }
    }
}
