using Azure.Core;
using CommandLine;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Performance.Application.DTO;
using Performance.Application.Queries;
using Performance.Domain.Entity;
using Performance.Application.Common;
using Performance.Application.Interface.Services;

namespace Performance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController (IUserServices _userServices, ILogger<UserController> _logger) 
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

                return Ok(await _userServices.GetPaginatedListAsync(request));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while processing pagination type {request.PaginationType}: ", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while processing your request: {ex.Message}");
            }
        }
    }
}
