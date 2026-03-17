using Microsoft.AspNetCore.Mvc;
using Performance.Application.DTO;
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
        public async Task<ActionResult<UserResponseDTO<User>>> GetPaginatedUsers([FromQuery] UserRequestDTO request)
        {
            return Ok(await userServices.GetPaginatedListAsync(request));
        }
    }
}
