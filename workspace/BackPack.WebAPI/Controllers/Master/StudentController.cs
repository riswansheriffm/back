using BackPack.Library.Constants;
using BackPack.Library.Messages;
using BackPack.Library.Requests.DBLog;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.Master.Student;
using BackPack.Library.Services.Interfaces.DBLog;
using BackPack.Library.Services.Interfaces.Master;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using BackPack.Dependency.Library.Swagger;
using BackPack.Dependency.Library.Messages;

namespace BackPack.WebAPI.Controllers.Master
{
    [Authorize]
    [Route(ControllerConstant.ServiceEndPoint)]
    [ApiController]
    [ApiExplorerSettings(GroupName = ControllerConstant.User)]
    public class StudentController(
        IDBLogService dbLogService,
        IStudentService studentService
        ) : BaseController(dbLogService)
    {
        #region GetStudent
        [Authorize(Roles = ControllerConstant.DistrictAdminRole + "," + ControllerConstant.SchoolAdminRole)]
        [Route("GetStudent")]
        [OperationOrder(03)]
        [HttpGet] 
        [SwaggerOperation(
            Summary = MasterMessage.StudentSummary,
            Description = MasterMessage.StudentDescription
        )]
        [ProducesResponseType(typeof(StudentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetStudent([Required][FromHeader] int LoginID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.AdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "LoginID: " + LoginID
            });

            var response = await studentService.GetStudentAsync(LoginID);

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

        #region GetAllStudent
        [Authorize(Roles = ControllerConstant.DistrictAdminRole + "," + ControllerConstant.SchoolAdminRole)]
        [Route("GetAllStudents")]
        [OperationOrder(04)]
        [HttpGet] 
        [SwaggerOperation(
            Summary = MasterMessage.AllStudentSummary,
            Description = MasterMessage.AllStudentDescription
        )]
        [ProducesResponseType(typeof(AllStudentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllStudent([Required][FromHeader] int DistrictID, [Required][FromHeader] int SchoolID) 
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = 0,
                LoginType = LogConstant.AdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "DomainID: " + DistrictID + ", SchoolID: " + SchoolID
            });

            var response = await studentService.GetAllStudentAsync(DistrictID, SchoolID);

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
