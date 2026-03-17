using Microsoft.AspNetCore.Mvc;
using Performance.API.Exceptions;
using Performance.Application.DTOs;
using Performance.Application.Interface.Services;
using Performance.Domain.Entity;

namespace Performance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserServices userServices)
        : ControllerBase
    {
        [HttpGet("getusers")]
        [ProducesResponseType(typeof(UserResponseDTO<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AppProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserResponseDTO<User>>> GetPaginatedUsers([FromQuery] UserRequestDTO request)
        {
            return Ok(await userServices.GetPaginatedListAsync(request));
        }
    }
}
