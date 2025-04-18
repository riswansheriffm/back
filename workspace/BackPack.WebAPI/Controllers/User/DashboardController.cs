using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Dependency.Library.Swagger;
using BackPack.Library.Constants;
using BackPack.Library.Messages;
using BackPack.Library.Requests.DBLog;
using BackPack.Library.Responses.Activity;
using BackPack.Library.Responses.Dashboard;
using BackPack.Library.Responses.User;
using BackPack.Library.Services.Interfaces.DBLog;
using BackPack.Library.Services.Interfaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace BackPack.WebAPI.Controllers.User
{
    [Authorize]
    [Route(ControllerConstant.ServiceEndPoint)]
    [ApiController]
    [ApiExplorerSettings(GroupName = ControllerConstant.User)]
    public class DashboardController(
        IDBLogService dbLogService,
        IDashboardService dashboardService,
        IClassRoomDashboardService classRoomDashboardService
        ) : BaseController(dbLogService)
    {
        #region GetBPStudentDashboard
        [Authorize(Roles = ControllerConstant.StudentRole)]
        [Route("GetBPStudentDashboard")]
        [OperationOrder(21)]
        [HttpGet]
        [SwaggerOperation(
            Summary = UserMessage.StudentDashboardSummary,
            Description = UserMessage.StudentDashboardDescription
        )]
        [ProducesResponseType(typeof(StudentDashboardResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetBPStudentDashboard([Required][FromHeader] int LoginID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "LoginID: " + LoginID
            });

            var response = await dashboardService.StudentDashboardResponseAsync(LoginID);

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

        #region GetBPUpcomingAssignments
        [Authorize(Roles = ControllerConstant.StudentRole)]
        [Route("GetBPUpcomingAssignments")]
        [OperationOrder(22)]
        [HttpGet]
        [SwaggerOperation(
            Summary = UserMessage.StudentDashboardUpcomingSummary,
            Description = UserMessage.StudentDashboardUpcomingDescription
        )]
        [ProducesResponseType(typeof(UpcomingAssignmentsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetBPUpcomingAssignments([Required][FromHeader] int StudentID, [Required][FromHeader] string AssignmentDate)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = StudentID,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "LoginID: " + StudentID + ", AssignmentDate: " + AssignmentDate
            });

            var response = await dashboardService.UpcomingAssignmentsResponseAsync(StudentID, AssignmentDate);

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

        #region GetDistrictAdminDashBoard
        [Authorize(Roles = ControllerConstant.DistrictAdminRole + "," + ControllerConstant.CurriculumAdminRole)]
        [Route("GetDistrictAdminDashBoard")]
        [OperationOrder(24)]
        [HttpGet] 
        [SwaggerOperation(
            Summary = UserMessage.StudentDashboardUpcomingSummary,
            Description = UserMessage.StudentDashboardUpcomingDescription  
        )]
        [ProducesResponseType(typeof(DistrictAdminDashboardResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDistrictAdminDashBoard([Required][FromHeader] int DistrictID, [Required][FromHeader] int LoginID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = 0,
                LoginType = LogConstant.AdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "DomainID: " + DistrictID + ", LoginID: " + LoginID
            });

            var response = await dashboardService.DistrictAdminDashboardAsync(DistrictID, LoginID); 

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

        #region GetCurriculumAdminDashBoard
        [Authorize(Roles = ControllerConstant.CurriculumAdminRole)]
        [Route("GetCurriculumAdminDashBoard")] 
        [OperationOrder(25)]
        [HttpGet]
        [SwaggerOperation(
            Summary = UserMessage.GetCurriculumAdminSummary,
            Description = UserMessage.GetCurriculumAdminDescription
        )]
        [ProducesResponseType(typeof(CurriculumAdminDashBoardResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCurriculumAdminDashBoard([Required][FromHeader] int DistrictID, [Required][FromHeader] int LoginID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = 0,
                LoginType = LogConstant.AdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "DomainID: " + DistrictID + ", LoginID: " + LoginID
            });

            var response = await dashboardService.CurriculumAdminDashboardAsync(DistrictID, LoginID);

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

        #region GetSchoolAdminDashboard
        [Authorize(Roles = ControllerConstant.SchoolAdminRole)]
        [Route("GetSchoolAdminDashboard")]
        [OperationOrder(26)]
        [HttpGet]
        [SwaggerOperation(
            Summary = UserMessage.SchoolAdminDashboardSummary,
            Description = UserMessage.SchoolAdminDashboardDescription
        )]
        [ProducesResponseType(typeof(SchoolAdminDashboardResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSchoolAdminDashboard([Required][FromHeader] int SchoolID, [Required][FromHeader] int LoginID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest 
            {
                LoginID = SchoolID, 
                LoginType = LogConstant.AdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "DomainID: " + SchoolID + ", LoginID: " + LoginID
            });

            var response = await dashboardService.SchoolAdminDashboardAsync(SchoolID, LoginID);

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

        #region GetCRTeacherDashboard
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetCRTeacherDashboard")]
        [OperationOrder(23)]
        [HttpGet]
        [SwaggerOperation(
            Summary = UserMessage.ClassRoomDashboardSummary,
            Description = UserMessage.ClassRoomDashboardDescription
        )]
        [ProducesResponseType(typeof(ClassRoomDashboardResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCRTeacherDashboard([Required][FromHeader] int LoginID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "LoginID: " + LoginID
            });

            var response = await classRoomDashboardService.ClassRoomDashboardResponseAsync(LoginID);

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

        #region GetCRUpcomingAssignments
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetCRUpcomingAssignments")]
        [OperationOrder(24)]
        [HttpGet]
        [SwaggerOperation(
            Summary = UserMessage.ClassRoomUpcomingAssignmentSummary,
            Description = UserMessage.ClassRoomUpcomingAssignmentDescription
        )]
        [ProducesResponseType(typeof(ClassRoomUpcomingAssignmentsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCRUpcomingAssignments([Required][FromHeader] int AuthorID, [Required][FromHeader] string AssignmentDate)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = AuthorID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "AuthorID: " + AuthorID + ", AssignmentDate: " + AssignmentDate
            });

            var response = await classRoomDashboardService.ClassRoomUpcomingAssignmentsResponseAsync(AuthorID, AssignmentDate);

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
