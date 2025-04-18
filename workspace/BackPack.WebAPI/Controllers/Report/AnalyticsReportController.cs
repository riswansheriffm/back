using BackPack.Library.Constants;
using BackPack.Library.Messages;
using BackPack.Library.Requests.DBLog;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.Report;
using BackPack.Library.Services.Interfaces.DBLog;
using BackPack.Library.Services.Interfaces.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using BackPack.Dependency.Library.Swagger;
using BackPack.Dependency.Library.Messages;

namespace BackPack.WebAPI.Controllers.Report
{
    [Authorize]
    [Route(ControllerConstant.ServiceEndPoint)]
    [ApiController]
    [ApiExplorerSettings(GroupName = ControllerConstant.AnalyticsReport)]
    public class AnalyticsReportController(
        IDBLogService dbLogService,
        IAnalyticsReportService analyticsReportService
        ) : BaseController(dbLogService)
    {
        #region GetCourseSummaryAnalyticsReport
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetCourseSummaryAnalyticsReport")]
        [OperationOrder(1)]
        [HttpGet]
        [SwaggerOperation(
            Summary = ReportMessage.CourseSummaryAnalyticsReportSummary,
            Description = ReportMessage.CourseSummaryAnalyticsReportDescription
        )]
        [ProducesResponseType(typeof(CourseSummaryAnalyticsReportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCourseSummaryAnalyticsReport([Required][FromHeader] int DomainID, [Required][FromHeader] int CourseID, [Required][FromHeader] int StudentID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = StudentID,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "DomainID: " + DomainID + ", " + ControllerConstant.CourseId + CourseID + ", " + ControllerConstant.StudentId + StudentID
            });

            var response = await analyticsReportService.CourseSummaryAnalyticsReportAsync(DomainID, CourseID, StudentID);

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

        #region GetLessonPodSummaryAnalyticsReport
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetLessonPodSummaryAnalyticsReport")]
        [OperationOrder(2)]
        [HttpGet]
        [SwaggerOperation(
            Summary = ReportMessage.LessonPodSummaryAnalyticsReportSummary,
            Description = ReportMessage.LessonPodSummaryAnalyticsReportDescription
        )]
        [ProducesResponseType(typeof(LessonPodSummaryAnalyticsReportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetLessonPodSummaryAnalyticsReport([Required][FromHeader] int LessonUnitDistID, [Required][FromHeader] int StudentID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = StudentID,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LessonUnitDistId + LessonUnitDistID + ", " + ControllerConstant.StudentId + StudentID
            });

            var response = await analyticsReportService.LessonPodSummaryAnalyticsReportAsync(LessonUnitDistID, StudentID);

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

        #region GetLessonPodActivityAnalyticsReport
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetLessonPodActivityAnalyticsReport")]
        [OperationOrder(23)]
        [HttpGet]
        [SwaggerOperation(
            Summary = ReportMessage.LessonPodActivityAnalyticsReportSummary,
            Description = ReportMessage.LessonPodActivityAnalyticsReportDescription
        )]
        [ProducesResponseType(typeof(LessonPodActivityAnalyticsReportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetLessonPodActivityAnalyticsReport([Required][FromHeader] int ContentID, [Required][FromHeader] int StudentID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = StudentID,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "ContentID: " + ContentID + ", " + ControllerConstant.StudentId + StudentID
            });

            var response = await analyticsReportService.LessonPodActivityAnalyticsReportAsync(ContentID, StudentID);

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

        #region GetAllChaptersBySubjectReport
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetAllChaptersBySubjectReport")]
        [OperationOrder(24)]
        [HttpGet]
        [SwaggerOperation(
            Summary = ReportMessage.AllChaptersBySubjectReportSummary,
            Description = ReportMessage.AllChaptersBySubjectReportDescription
        )]
        [ProducesResponseType(typeof(AllChaptersBySubjectReportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllChaptersBySubjectReport([Required][FromHeader] int SubjectID, [Required][FromHeader] int CourseID, [Required][FromHeader] int LoginID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID + ", SubjectID: " + SubjectID + ", " + ControllerConstant.CourseId + CourseID
            });

            var response = await analyticsReportService.AllChaptersBySubjectReportAsync(SubjectID, CourseID);

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

        #region GetCRLessonPodSummaryReport
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetCRLessonPodSummaryReport")]
        [OperationOrder(25)]
        [HttpGet]
        [SwaggerOperation(
            Summary = ReportMessage.CRLessonPodSummaryReportSummary,
            Description = ReportMessage.CRLessonPodSummaryReportDescription
        )]
        [ProducesResponseType(typeof(CRLessonPodSummaryReportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCRLessonPodSummaryReport([Required][FromHeader] int LoginID, [Required][FromHeader] int LessonUnitDistID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID + ", " + ControllerConstant.LessonUnitDistId + LessonUnitDistID
            });

            var response = await analyticsReportService.CRLessonPodSummaryReportAsync(LoginID, LessonUnitDistID);

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

        #region GetCRLessonPodActivityReport
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetCRLessonPodActivityReport")]
        [OperationOrder(26)]
        [HttpGet]
        [SwaggerOperation(
            Summary = ReportMessage.CRLessonPodActivityReportSummary,
            Description = ReportMessage.CRLessonPodActivityReportDescription
        )]
        [ProducesResponseType(typeof(CRLessonPodActivityReportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCRLessonPodActivityReport([Required][FromHeader] int LoginID, [Required][FromHeader] int ContentID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID + ", ContentID: " + ContentID
            });

            var response = await analyticsReportService.CRLessonPodActivityReportAsync(LoginID, ContentID);

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

        #region GetCRLessonPodActivityDetailReport
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetCRLessonPodActivityDetailReport")]
        [OperationOrder(27)]
        [HttpGet]
        [SwaggerOperation(
            Summary = ReportMessage.CRLessonPodActivityDetailReportSummary,
            Description = ReportMessage.CRLessonPodActivityDetailReportDescription
        )]
        [ProducesResponseType(typeof(CRLessonPodActivityDetailReportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCRLessonPodActivityDetailReport([Required][FromHeader] int LoginID, [Required][FromHeader] int ContentID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID + ", ContentID: " + ContentID
            });

            var response = await analyticsReportService.CRLessonPodActivityDetailReportAsync(LoginID, ContentID);

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

        #region GetClassLevelCourseSummaryAnalyticsReport
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetClassLevelCourseSummaryAnalyticsReport")]
        [OperationOrder(28)]
        [HttpGet]
        [SwaggerOperation(
            Summary = ReportMessage.ClassLevelCourseSummaryAnalyticReportSummary,
            Description = ReportMessage.ClassLevelCourseSummaryAnalyticReportDescription
        )]
        [ProducesResponseType(typeof(ClassLevelCourseSummaryAnalyticsReportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetClassLevelCourseSummaryAnalyticsReport([Required][FromHeader] int DomainID, [Required][FromHeader] int CourseID, [Required][FromHeader] int LoginID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID + ", DomainID: " + DomainID + ", " + ControllerConstant.CourseId + CourseID
            });

            var response = await analyticsReportService.ClassLevelCourseSummaryAnalyticsReportAsync(DomainID, CourseID);

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

        #region GetCRDistLessonPodTierSummaryReport
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetCRDistLessonPodTierSummaryReport")]
        [OperationOrder(29)]
        [HttpGet]
        [SwaggerOperation(
            Summary = ReportMessage.CRDistLessonPodTierSummaryReportSummary,
            Description = ReportMessage.CRDistLessonPodTierSummaryReportDescription
        )]
        [ProducesResponseType(typeof(CRDistLessonPodSummaryTierReportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCRDistLessonPodTierSummaryReport([Required][FromHeader] int LoginID, [Required][FromHeader] int LessonUnitDistID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID + ", " + ControllerConstant.LessonUnitDistId + LessonUnitDistID
            });

            var response = await analyticsReportService.CRDistLessonPodSummaryTierReportAsync(LoginID, LessonUnitDistID);

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

        #region GetCRDistLessonPodTierStudentActivityReport
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetCRDistLessonPodTierStudentActivityReport")]
        [OperationOrder(30)]
        [HttpGet]
        [SwaggerOperation(
            Summary = ReportMessage.CRDistLessonPodTierStudentActivityReportSummary,
            Description = ReportMessage.CRDistLessonPodTierStudentActivityReportDescription
        )]
        [ProducesResponseType(typeof(CRDistLessonPodTierStudentActivityReportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCRDistLessonPodTierStudentActivityReport([Required][FromHeader] int LoginID, [Required][FromHeader] int LessonUnitDistID, [Required][FromHeader] string TierName, [Required][FromHeader] int MinRange, [Required][FromHeader] int MaxRange)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID + ", " + ControllerConstant.LessonUnitDistId + LessonUnitDistID + ", TierName: " + TierName + ", MinRange: " + MinRange + ", MaxRange: " + MaxRange
            });

            var response = await analyticsReportService.CRDistLessonPodTierStudentActivityReportAsync(LoginID, LessonUnitDistID, TierName, MinRange, MaxRange);

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

        #region GetCRSubjectSummaryTagReport
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetCRSubjectSummaryTagReport")]
        [OperationOrder(31)]
        [HttpGet]
        [SwaggerOperation(
            Summary = ReportMessage.CRSubjectSummaryTagReportSummary,
            Description = ReportMessage.CRSubjectSummaryTagReportDescription
        )]
        [ProducesResponseType(typeof(CRSubjectSummaryTagReportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCRSubjectSummaryTagReport([Required][FromHeader] int LoginID, [Required][FromHeader] int SubjectID, [Required][FromHeader] int StudentID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID + ", SubjectID: " + SubjectID + ", " + ControllerConstant.StudentId + StudentID
            });

            var response = await analyticsReportService.CRSubjectSummaryTagReportAsync(SubjectID, StudentID);

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

        #region GetCRLessonFolderSummaryTagReport
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetCRLessonFolderSummaryTagReport")]
        [OperationOrder(32)]
        [HttpGet]
        [SwaggerOperation(
            Summary = ReportMessage.CRLessonFolderSummaryTagReportSummary,
            Description = ReportMessage.CRLessonFolderSummaryTagReportDescription
        )]
        [ProducesResponseType(typeof(CRLessonFolderSummaryTagReportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCRLessonFolderSummaryTagReport([Required][FromHeader] int LoginID, [Required][FromHeader] int LessonID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID + ", LessonID: " + LessonID
            });

            var response = await analyticsReportService.CRLessonFolderSummaryTagReportAsync(LessonID);

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

        #region GetCRChapterSummaryTagReport
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetCRChapterSummaryTagReport")]
        [OperationOrder(33)]
        [HttpGet]
        [SwaggerOperation(
            Summary = ReportMessage.CRChapterSummaryTagReportSummary,
            Description = ReportMessage.CRChapterSummaryTagReportDescription
        )]
        [ProducesResponseType(typeof(CRChapterSummaryTagReportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCRChapterSummaryTagReport([Required][FromHeader] int LoginID, [Required][FromHeader] int ChapterID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID + ", ChapterID: " + ChapterID
            });

            var response = await analyticsReportService.CRChapterSummaryTagReportAsync(ChapterID);

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

        #region GetCRLessonFoldersBySubjectReport
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetCRLessonFoldersBySubjectReport")]
        [OperationOrder(33)]
        [HttpGet]
        [SwaggerOperation(
            Summary = ReportMessage.CRLessonFoldersBySubjectReportSummary,
            Description = ReportMessage.CRLessonFoldersBySubjectReportDescription
        )]
        [ProducesResponseType(typeof(CRLessonFoldersBySubjectReportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCRLessonFoldersBySubjectReport([Required][FromHeader] int DomainID, [Required][FromHeader] int AuthorID, [Required][FromHeader] int SubjectID, [Required][FromHeader] int CourseID, [Required][FromHeader] int ChapterID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = AuthorID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "DomainID: " + DomainID + ", AuthorID: " + AuthorID + ", SubjectID: " + SubjectID + ", " + ControllerConstant.CourseId + CourseID + ", ChapterID: " + ChapterID
            });

            var response = await analyticsReportService.CRLessonFoldersBySubjectReportAsync(DomainID, AuthorID, SubjectID, CourseID, ChapterID);

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

        #region GetCRDistributedLessonUnitsByLessonReport
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetCRDistributedLessonUnitsByLessonReport")]
        [OperationOrder(33)]
        [HttpGet]
        [SwaggerOperation(
            Summary = ReportMessage.CRDistributedLessonUnitsByLessonReportSummary,
            Description = ReportMessage.CRDistributedLessonUnitsByLessonReportDescription
        )]
        [ProducesResponseType(typeof(CRDistributedLessonUnitsByLessonReportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCRDistributedLessonUnitsByLessonReport([Required][FromHeader] int LoginID, [Required][FromHeader] int LessonID, [Required][FromHeader] int CourseID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID + ", LessonID: " + LessonID + ", " + ControllerConstant.CourseId + CourseID
            });

            var response = await analyticsReportService.CRDistributedLessonUnitsByLessonReportAsync(LoginID, LessonID, CourseID);

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

        #region GetCRClassSummaryTagReport
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetCRClassSummaryTagReport")]
        [OperationOrder(34)]
        [HttpGet]
        [SwaggerOperation(
            Summary = ReportMessage.CRClassSummaryTagReportSummary,
            Description = ReportMessage.CRClassSummaryTagReportDescription
        )]
        [ProducesResponseType(typeof(CRClassSummaryTagReportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCRClassSummaryTagReport([Required][FromHeader] int LoginID, [Required][FromHeader] int CourseID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID + ", " + ControllerConstant.CourseId + CourseID
            });

            var response = await analyticsReportService.CRClassSummaryTagReportAsync(CourseID);

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

        #region GetCRStudentList
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetCRStudentList")]
        [OperationOrder(35)]
        [HttpGet]
        [SwaggerOperation(
            Summary = ReportMessage.CRStudentListReportSummary,
            Description = ReportMessage.CRStudentListReportDescription
        )]
        [ProducesResponseType(typeof(CRStudentListReportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCRStudentList([Required][FromHeader] int LessonUnitDistID, [Required][FromHeader] int LoginID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID + ", " + ControllerConstant.LessonUnitDistId + LessonUnitDistID
            });

            var response = await analyticsReportService.CRStudentListReportAsync(LessonUnitDistID, LoginID);

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

        #region GetCRStudentWork
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetCRStudentWork")]
        [OperationOrder(36)]
        [HttpGet]
        [SwaggerOperation(
            Summary = ReportMessage.CRStudentWorkReportSummary,
            Description = ReportMessage.CRStudentWorkReportDescription
        )]
        [ProducesResponseType(typeof(CRStudentWorkReportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCRStudentWork([Required][FromHeader] int LessonUnitDistID, [Required][FromHeader] int StudentID, [Required][FromHeader] int LoginID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID + ", " + ControllerConstant.LessonUnitDistId + LessonUnitDistID + ", " + ControllerConstant.StudentId + StudentID
            });

            var response = await analyticsReportService.CRStudentWorkReportAsync(LessonUnitDistID, StudentID, LoginID);

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
