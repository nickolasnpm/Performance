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
        [ProducesResponseType(typeof(AppProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AppProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserResponseDTO<UserDTO>>> GetPaginatedUsers([FromQuery] UserRequestDTO request)
        {
            return Ok(await userServices.GetPaginatedListAsync(request));
        }
    }
}
