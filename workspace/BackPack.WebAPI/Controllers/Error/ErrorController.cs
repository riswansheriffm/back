
using BackPack.Dependency.Library.Responses;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using BackPack.Dependency.Library.Messages;
using BackPack.Library.Services.Interfaces.DBLog;

namespace BackPack.WebAPI.Controllers.Error
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController(
        IDBLogService dbLogService
        ) : BaseController(dbLogService)
    {
        [Route("{StatusCode}")]
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HttpStatusCodeHandler(int StatusCode)
        {
            Log.Error("{Message}-{StatusCode}-{ErrorType}-{ErrorMessage}", CommonMessage.UnauthorizedMessage, StatusCode, "API Transactional Error", CommonMessage.UnauthorizedMessage);
            return Unauthorized(new UnauthorizedResponse
            {
                MessageID = CommonMessage.FailID,
                Success = false,
                StatusCode = StatusCode,
                StatusMessage = CommonMessage.UnauthorizedMessage
            });
        }
    }
}
