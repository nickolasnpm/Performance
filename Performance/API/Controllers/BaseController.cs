using Microsoft.AspNetCore.Mvc;
using Performance.API.Exceptions;
using Performance.Application.Common.Models;

namespace Performance.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected ActionResult ToProblem(ResultError error)
        {
            var problem = ToProblemResult.FromResultError(error);
            return StatusCode(problem.Status ?? 500, problem);
        }
    }
}