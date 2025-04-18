
using BackPack.Dependency.Library.Responses;
using BackPack.Dependency.Library.Swagger;
using BackPack.Library.Constants;
using BackPack.Library.Services.Interfaces.DBLog;
using Microsoft.AspNetCore.Mvc;

namespace BackPack.WebAPI.Controllers.Error
{
    [Route(ControllerConstant.ServiceEndPoint)]
    [ApiController]
    [ApiExplorerSettings(GroupName = ControllerConstant.ServiceStatus)]
    public class ServiceTestController(
        IDBLogService dbLogService
        ) : BaseController(dbLogService)
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
