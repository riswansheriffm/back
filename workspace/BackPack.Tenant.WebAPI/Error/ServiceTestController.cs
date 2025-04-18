using BackPack.Dependency.Library.Responses;
using BackPack.Dependency.Library.Swagger;
using BackPack.Tenant.Library.Constants;
using Microsoft.AspNetCore.Mvc;

namespace BackPack.Tenant.WebAPI.Error
{
    [Route(ControllerConstant.ServiceEndPoint)]
    [ApiController]
    [ApiExplorerSettings(GroupName = ControllerConstant.ServiceStatus)]
    public class ServiceTestController : ControllerBase
    {
        [Route("ServiceStatus")]
        [OperationOrder(1)]
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ServiceStatus()
        {
            return StatusCode(StatusCodes.Status200OK, new BaseResponse
            {
                MessageID = 0,
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Success"
            });
        }
    }
}
