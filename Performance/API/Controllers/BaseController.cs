using Microsoft.AspNetCore.Mvc;
using Performance.API.Exceptions;
using Performance.Application.Common.Models;

namespace Performance.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected const string _bulkRoute = "bulk";

        protected ActionResult<TSuccess> ToResponse<TSuccess, TError>(Result<TSuccess, TError> result)
            where TError : ResultError
        {
            if (result.IsSuccess)
                return Ok(result.Data);

            var problem = ToProblemResult.FromResultError(result.Error!);
            return StatusCode(problem.Status ?? 500, problem);
        }
    }
}