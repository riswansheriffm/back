using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Tenant.Library.Requests;
using BackPack.Dependency.Library.Swagger;
using BackPack.MessageContract.Library;
using BackPack.Tenant.Library.Constants;
using BackPack.Tenant.Library.Messages;
using BackPack.Tenant.Library.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace BackPack.Tenant.WebAPI.Controllers
{
    [Authorize]
    [Route(ControllerConstant.ServiceEndPoint)]
    [ApiController]
    [ApiExplorerSettings(GroupName = ControllerConstant.Dashboard)]
    public class DashboardController(
        IDBLogService dbLogService,
        IDashboardService dashboardService
        ) : BaseController(dbLogService)
    {
        #region GetSuperAdminDashboard
        [Route("GetSuperAdminDashboard")]
        [OperationOrder(23)]
        [HttpGet]
        [SwaggerOperation(
            Summary = UserMessage.GetSuperAdminSummary,
            Description = UserMessage.GetSuperAdminDescription
        )]
        [ProducesResponseType(typeof(GetSuperAdminDashboardAcceptedEvent), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSuperAdminDashboard()
        {            
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginType = LogConstant.SuperAdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
            });

            var response = await dashboardService.SuperAdminDashboardAsync();

            await UpdateDBLogAsync(new DBLogRequest
            {
                ServiceLogID = guid,
                ServiceResponse = JsonSerializer.Serialize(response),
                ServiceStatus = (!response.Success) ? CommonMessage.ServiceStatusFail : CommonMessage.ServiceStatusSuccess,
                LogMessage = response.StatusMessage!,
                ExceptionType = response.ExceptionType!,
                ExceptionMessage = response.ExceptionMessage!,
                Success = response.Success
            });

            #region Response
            if (response.StatusCode == StatusCodes.Status200OK)
            {
                return StatusCode(StatusCodes.Status200OK, response);
            }
            else
            {
                return StatusCode(response.StatusCode, response);
            }
            #endregion
        }
        #endregion
    }
}
