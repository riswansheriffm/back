using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Dependency.Library.Swagger;
using BackPack.Library.Constants;
using BackPack.Library.Messages;
using BackPack.Library.Requests.DBLog;
using BackPack.Library.Responses.Master.Course;
using BackPack.Library.Services.Interfaces.CourseCapsule;
using BackPack.Library.Services.Interfaces.DBLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace BackPack.WebAPI.Controllers.CourseCapsule
{
    [Authorize]
    [Route(ControllerConstant.ServiceEndPoint)]
    [ApiController]
    [ApiExplorerSettings(GroupName = ControllerConstant.CourseCapsule)]
    public class CourseCapsuleGetController(
        IDBLogService dbLogService,
        ICourseCapsuleGetService courseCapsuleGetService
        ) : BaseController(dbLogService)
    {
        #region GetAllCourseCapsules
        [Authorize(Roles = ControllerConstant.CurriculumAdminRole)]
        [Route("GetAllCourseCapsules")]
        [OperationOrder(07)]
        [HttpGet]
        [SwaggerOperation(
            Summary = MasterMessage.GetAllCourseCapsulesSummary,
            Description = MasterMessage.GetAllCourseCapsulesDescription
        )]
        [ProducesResponseType(typeof(AllCourseCapsulesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllCourseCapsules([Required][FromHeader] int LoginID, [Required][FromHeader] int SubjectID, [Required][FromHeader] string AppType)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.AdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID
            });

            var response = await courseCapsuleGetService.GetAllCourseCapsulesAsync(LoginID, SubjectID, AppType);

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

        #region GetAllCourseCapsulesForASubject
        [Authorize(Roles = ControllerConstant.CurriculumAdminRole)]
        [Route("GetAllCourseCapsulesForASubject")]
        [OperationOrder(09)]
        [HttpGet]
        [SwaggerOperation(
            Summary = MasterMessage.GetAllCourseCapsulesForASubjectSummary,
            Description = MasterMessage.GetAllCourseCapsulesForASubjectDescription
        )]
        [ProducesResponseType(typeof(AllCourseCapsulesForASubjectResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllCourseCapsulesForASubject([Required][FromHeader] int LoginID, [Required][FromHeader] int SubjectID, [Required][FromHeader] string AppType)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.AdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID
            });

            var response = await courseCapsuleGetService.GetAllCourseCapsulesForASubjectAsync(LoginID, SubjectID, AppType);

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

        #region GetAllCourseCapsuleFoldersForACourseCapsule
        [Route("GetAllCourseCapsuleFoldersForACourseCapsule")]
        [OperationOrder(10)]
        [HttpGet]
        [SwaggerOperation(
             Summary = MasterMessage.GetAllCourseCapsuleFoldersForACourseCapsuleSummary,
             Description = MasterMessage.GetAllCourseCapsuleFoldersForACourseCapsuleDescription
        )]
        [ProducesResponseType(typeof(FoldersForACourseCapsuleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllCourseCapsuleFoldersForACourseCapsule([Required][FromHeader] int CourseCapsuleID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = CourseCapsuleID,
                LoginType = LogConstant.AdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "CourseCapsuleID: " + CourseCapsuleID
            });

            var response = await courseCapsuleGetService.GetAllCourseCapsuleFoldersForACourseCapsuleAsync(CourseCapsuleID);

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

        #region GetAllPublicCourseCapsuleByDomainAndSubject
        [Route("GetAllPublicCourseCapsuleByDomainAndSubject")]
        [OperationOrder(11)]
        [HttpGet]
        [SwaggerOperation(
             Summary = MasterMessage.GetAllPublicCourseCapsuleByDomainAndSubjectSummary,
             Description = MasterMessage.GetAllPublicCourseCapsuleByDomainAndSubjectDescription
        )]
        [ProducesResponseType(typeof(AllCourseCapsulesForASubjectResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllPublicCourseCapsuleByDomainAndSubject([Required][FromHeader] int DomainID, [Required][FromHeader] int SubjectID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = DomainID,
                LoginType = LogConstant.AdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "DomainID: " + DomainID
            });

            var response = await courseCapsuleGetService.GetAllPublicCourseCapsuleByDomainAndSubjectAsync(DomainID, SubjectID);

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

        #region GetAllLPStudentLicensesByCourseCapsule
        [Authorize(Roles = ControllerConstant.CurriculumAdminRole)]
        [Route("GetAllLPStudentLicensesByCourseCapsule")]
        [OperationOrder(12)]
        [HttpGet]
        [SwaggerOperation(
            Summary = MasterMessage.GetAllLPStudentLicensesByCourseCapsuleSummary,
            Description = MasterMessage.GetAllLPStudentLicensesByCourseCapsuleDescription
        )]
        [ProducesResponseType(typeof(LPStudentLicensesByCourseCapsuleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllLPStudentLicensesByCourseCapsule([Required][FromHeader] int LoginID, [Required][FromHeader] int CourseID, [Required][FromHeader] int CourseCapsuleID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.AdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID
            });

            var response = await courseCapsuleGetService.GetAllLPStudentLicensesByCourseCapsuleAsync(LoginID, CourseID, CourseCapsuleID);

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

        #region GetAllLPCourseLicenses
        [Authorize(Roles = ControllerConstant.CurriculumAdminRole)]
        [Route("GetAllLPCourseLicenses")]
        [OperationOrder(13)]
        [HttpGet]
        [SwaggerOperation(
            Summary = MasterMessage.GetAllLPCourseLicensesSummary,
            Description = MasterMessage.GetAllLPCourseLicensesDescription
        )]
        [ProducesResponseType(typeof(LPCourseLicensesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllLPCourseLicenses([Required][FromHeader] int LoginID, [Required][FromHeader] int CourseID, [Required][FromHeader] int CourseCapsuleID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.AdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID
            });

            var response = await courseCapsuleGetService.GetAllLPCourseLicensesAsync(LoginID, CourseID, CourseCapsuleID);

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

        #region GetCourseCapsuleForRecorder
        [Authorize(Roles = ControllerConstant.CurriculumAdminRole)]
        [Route("GetCourseCapsuleForRecorder")]
        [OperationOrder(14)]
        [HttpGet]
        [SwaggerOperation(
            Summary = MasterMessage.GetCourseCapsuleForReorderSummary,
            Description = MasterMessage.GetCourseCapsuleForReorderDescription
        )]
        [ProducesResponseType(typeof(CourseCapsuleForReorderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCourseCapsuleForRecorder([Required][FromHeader] int LoginID, [Required][FromHeader] int SubjectID, [Required][FromHeader] int CourseCapsuleID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.AdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID
            });

            var response = await courseCapsuleGetService.GetCourseCapsuleForRecorderAsync(LoginID, SubjectID, CourseCapsuleID);

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
