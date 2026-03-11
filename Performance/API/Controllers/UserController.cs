using Microsoft.AspNetCore.Mvc;
using Performance.Application.DTO;
using Performance.Application.Interface.Services;

namespace Performance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController (IUserServices userServices, ILogger<UserController> logger) 
        : ControllerBase
    {
        [HttpGet("getusers")]
        public async Task<IActionResult> GetPaginatedUsers ([FromQuery] UserRequestDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return Ok(await userServices.GetPaginatedListAsync(request));
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occured while processing pagination type {request.PaginationType}: ", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while processing your request: {ex.Message}");
            }
        }
    }
}
