using BackPack.Library.Constants;
using BackPack.Library.Messages;
using BackPack.Library.Requests.DBLog;
using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.LessonPod;
using BackPack.Library.Services.Interfaces.DBLog;
using BackPack.Library.Services.Interfaces.LessonPod;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Swagger;

namespace BackPack.WebAPI.Controllers.LessonPod
{
    [Authorize]
    [Route(ControllerConstant.ServiceEndPoint)]
    [ApiController]
    [ApiExplorerSettings(GroupName = ControllerConstant.LessonPod)]
    public class LessonPodController(
        IDBLogService dbLogService,
        IValidator<CreateLessonPodRequest> createLessonPodValidator,
        IValidator<LessonPodPropertiesRequest> lessonPodPropertiesValidator,
        IValidator<CopyLessonPodRequest> copyLessonPodRequestValidator,
        IValidator<DeleteLessonPodRequest> deleteLessonPodValidator,               
        ILessonPodService lessonPodService,        
        ISyncLessonPodService syncLessonPodService        
        ) : BaseController(dbLogService)
    {
        #region CreateLessonUnit
        [Authorize(Roles = ControllerConstant.TeacherRole + "," + ControllerConstant.CurriculumAdminRole)]
        [Route("CreateLessonUnit")]
        [OperationOrder(7)]
        [HttpPost]
        [SwaggerOperation(
            Summary = LessonPodMessage.CreateLessonPodSummary,
            Description = LessonPodMessage.CreateLessonPodDescription
        )]
        [ProducesResponseType(typeof(CreateLessonPodResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(InternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateLessonUnit(CreateLessonPodRequest request)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = request.AuthorID,
                LoginType = LogConstant.StudioLoginType,
                ServiceType = LogConstant.Post,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = JsonSerializer.Serialize(request)
            });

            #region Validation
            var validation = await createLessonPodValidator.ValidateAsync(request);
            if (!validation.IsValid && validation.Errors.Any())
            {
                var validationResponse = await ValidationResponse(validation, guid);
                return new BadRequestObjectResult(validationResponse);
            }
            #endregion

            CreateLessonPodResponse response = await lessonPodService.CreateLessonPodAsync(request);

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
            if (response.StatusCode == StatusCodes.Status201Created)
            {
                return StatusCode(StatusCodes.Status201Created, response);
            }

            if (response.StatusCode == StatusCodes.Status400BadRequest)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BadRequestResponse
                {
                    MessageID = response.MessageID,
                    Success = response.Success,
                    StatusCode = response.StatusCode,
                    StatusMessage = response.StatusMessage
                });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new InternalServerErrorResponse
            {
                MessageID = CommonMessage.ErrorID,
                Success = response.Success,
                StatusCode = StatusCodes.Status500InternalServerError,
                StatusMessage = CommonMessage.InternalServerErrorMessage
            });
            #endregion
        }
        #endregion

        #region UpdateLessonUnitProperties
        [Authorize(Roles = ControllerConstant.TeacherRole + "," + ControllerConstant.CurriculumAdminRole)]
        [Route("UpdateLessonUnitProperties")]
        [OperationOrder(6)]
        [HttpPost]
        [SwaggerOperation(
            Summary = LessonPodMessage.UpdateLessonPodPodPropertiesSummary,
            Description = LessonPodMessage.UpdateLessonPodPodPropertiesDescription
        )]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(InternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateLessonUnitProperties(LessonPodPropertiesRequest request)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = request.AuthorID,
                LoginType = LogConstant.StudioLoginType,
                ServiceType = LogConstant.Post,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = JsonSerializer.Serialize(request)
            });

            #region Validation
            var validation = await lessonPodPropertiesValidator.ValidateAsync(request);
            if (!validation.IsValid && validation.Errors.Any())
            {
                var validationResponse = await ValidationResponse(validation, guid);
                return new BadRequestObjectResult(validationResponse);
            }
            #endregion

            BaseResponse response = await lessonPodService.UpdateLessonPodPropertiesAsync(request);

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
            if (response.StatusCode == StatusCodes.Status201Created)
            {
                return StatusCode(StatusCodes.Status201Created, response);
            }

            if (response.StatusCode == StatusCodes.Status400BadRequest)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BadRequestResponse
                {
                    MessageID = response.MessageID,
                    Success = response.Success,
                    StatusCode = response.StatusCode,
                    StatusMessage = response.StatusMessage
                });
            }

            if (response.StatusCode == StatusCodes.Status500InternalServerError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new InternalServerErrorResponse
                {
                    MessageID = response.MessageID,
                    Success = response.Success,
                    StatusCode = response.StatusCode,
                    StatusMessage = response.StatusMessage
                });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new InternalServerErrorResponse
            {
                MessageID = CommonMessage.ErrorID,
                Success = response.Success,
                StatusCode = StatusCodes.Status500InternalServerError,
                StatusMessage = CommonMessage.InternalServerErrorMessage
            });
            #endregion
        }
        #endregion

        #region CopyLessonUnit
        [Authorize(Roles = ControllerConstant.TeacherRole + "," + ControllerConstant.CurriculumAdminRole)]
        [Route("CopyLessonUnit")]
        [OperationOrder(7)]
        [HttpPost]
        [SwaggerOperation(
            Summary = LessonPodMessage.CopyLessonPodSummary,
            Description = LessonPodMessage.CopyLessonPodDescription
        )]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(InternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CopyLessonUnit(CopyLessonPodRequest request)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = request.AuthorID,
                LoginType = LogConstant.StudioLoginType,
                ServiceType = LogConstant.Post,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = JsonSerializer.Serialize(request)
            });

            #region Validation
            var validation = await copyLessonPodRequestValidator.ValidateAsync(request);
            if (!validation.IsValid && validation.Errors.Any())
            {
                var validationResponse = await ValidationResponse(validation, guid);
                return new BadRequestObjectResult(validationResponse);
            }
            #endregion

            BaseResponse response = await lessonPodService.CopyLessonPodAsync(request);

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
            if (response.StatusCode == StatusCodes.Status201Created)
            {
                return StatusCode(StatusCodes.Status201Created, response);
            }

            if (response.StatusCode == StatusCodes.Status400BadRequest)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BadRequestResponse
                {
                    MessageID = response.MessageID,
                    Success = response.Success,
                    StatusCode = response.StatusCode,
                    StatusMessage = response.StatusMessage
                });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new InternalServerErrorResponse
            {
                MessageID = CommonMessage.ErrorID,
                Success = response.Success,
                StatusCode = StatusCodes.Status500InternalServerError,
                StatusMessage = CommonMessage.InternalServerErrorMessage
            });
            #endregion
        }
        #endregion

        #region DeleteLessonUnit
        [Authorize(Roles = ControllerConstant.TeacherRole + "," + ControllerConstant.CurriculumAdminRole)]
        [Route("DeleteLessonUnit")]
        [OperationOrder(8)]
        [HttpPost]
        [SwaggerOperation(
            Summary = LessonPodMessage.DeleteLessonPodSummary,
            Description = LessonPodMessage.DeleteLessonPodDescription
        )]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(InternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteLessonUnit(DeleteLessonPodRequest request)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = request.AuthorID,
                LoginType = LogConstant.StudioLoginType,
                ServiceType = LogConstant.Post,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = JsonSerializer.Serialize(request)
            });

            #region Validation
            var validation = await deleteLessonPodValidator.ValidateAsync(request);
            if (!validation.IsValid && validation.Errors.Any())
            {
                var validationResponse = await ValidationResponse(validation, guid);
                return new BadRequestObjectResult(validationResponse);
            }
            #endregion

            BaseResponse response = await lessonPodService.DeleteLessonPodAsync(request);

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
            if (response.StatusCode == StatusCodes.Status201Created)
            {
                return StatusCode(StatusCodes.Status201Created, response);
            }

            if (response.StatusCode == StatusCodes.Status400BadRequest)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new BadRequestResponse
                {
                    MessageID = response.MessageID,
                    Success = response.Success,
                    StatusCode = response.StatusCode,
                    StatusMessage = response.StatusMessage
                });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new InternalServerErrorResponse
            {
                MessageID = CommonMessage.ErrorID,
                Success = response.Success,
                StatusCode = StatusCodes.Status500InternalServerError,
                StatusMessage = CommonMessage.InternalServerErrorMessage
            });
            #endregion
        }
        #endregion
                                
        #region GetPendingLessonUnitsForAStudent
        [Authorize(Roles = ControllerConstant.StudentRole)]
        [Route("GetPendingLessonUnitsForAStudent")]
        [OperationOrder(42)]
        [HttpGet]
        [SwaggerOperation(
            Summary = LessonPodMessage.PendingLessonPodsForAStudentSummary,
            Description = LessonPodMessage.PendingLessonPodsForAStudentDescription
        )]
        [ProducesResponseType(typeof(PendingLessonPodsForAStudentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPendingLessonUnitsForAStudent([Required][FromHeader] int StudentID, [Required][FromHeader] int CourseID, [FromHeader] int ParentID, [Required][FromHeader] int LessonID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = StudentID,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.StudentId + StudentID + ", CourseID: " + CourseID + ", " + ControllerConstant.ParentId + ParentID + ", LessonID: " + LessonID
            });

            var response = await lessonPodService.PendingLessonPodsForAStudentAsync(StudentID, CourseID, ParentID, LessonID);
            
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

        #region GetBPCompletedLessonUnitsByLesson
        [Authorize(Roles = ControllerConstant.StudentRole)]
        [Route("GetBPCompletedLessonUnitsByLesson")]
        [OperationOrder(43)]
        [HttpGet]
        [SwaggerOperation(
            Summary = LessonPodMessage.CompletedLessonPodsByLessonSummary,
            Description = LessonPodMessage.CompletedLessonPodsByLessonDescription
        )]
        [ProducesResponseType(typeof(CompletedLessonPodsByLessonResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetBPCompletedLessonUnitsByLesson([Required][FromHeader] int StudentID, [Required][FromHeader] int LessonID, [FromHeader] int ParentID, [Required][FromHeader] int ChapterID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = StudentID,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.StudentId + StudentID + ", " + ControllerConstant.LessonId + LessonID + ", " + ControllerConstant.ParentId + ParentID + ", ChapterID: " + ChapterID
            });

            var response = await lessonPodService.CompletedLessonPodsByLessonAsync(StudentID, LessonID, ParentID, ChapterID);

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

        #region GetLessonUnitSummaryForAStudent
        [Authorize(Roles = ControllerConstant.StudentRole)]
        [Route("GetLessonUnitSummaryForAStudent")]
        [OperationOrder(44)]
        [HttpGet]
        [SwaggerOperation(
            Summary = LessonPodMessage.LessonPodSummaryForAStudentSummary,
            Description = LessonPodMessage.LessonPodSummaryForAStudentDescription
        )]
        [ProducesResponseType(typeof(LessonPodSummaryForAStudentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetLessonUnitSummaryForAStudent([Required][FromHeader] int StudentID, [Required][FromHeader] int LessonUnitDistID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = StudentID,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.StudentId + StudentID + ", " + ControllerConstant.LessonUnitDistId + LessonUnitDistID
            });

            var response = await lessonPodService.LessonPodSummaryForAStudentAsync(StudentID, LessonUnitDistID);

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

        #region GetLessonUnitDetailsForAStudent
        [Authorize(Roles = ControllerConstant.StudentRole)]
        [Route("GetLessonUnitDetailsForAStudent")]
        [OperationOrder(45)]
        [HttpGet]
        [SwaggerOperation(
            Summary = LessonPodMessage.LessonPodDetailsForAStudentSummary,
            Description = LessonPodMessage.LessonPodDetailsForAStudentDescription
        )]
        [ProducesResponseType(typeof(LessonPodDetailsForAStudentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetLessonUnitDetailsForAStudent([Required][FromHeader] int StudentID, [Required][FromHeader] int LessonUnitDistID, [FromHeader] int ParentID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = StudentID,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.StudentId + StudentID + ", " + ControllerConstant.LessonUnitDistId + LessonUnitDistID + ", " + ControllerConstant.ParentId + ParentID
            });

            var response = await lessonPodService.LessonPodDetailsForAStudentAsync(StudentID, LessonUnitDistID, ParentID);

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

        #region GetAllDistributedLessonodsByTeacherByLessonPod
        [Authorize(Roles = ControllerConstant.TeacherRole + "," + ControllerConstant.CurriculumAdminRole)]
        [Route("GetAllDistributedLessonodsByTeacherByLessonPod")]
        [OperationOrder(46)]
        [HttpGet]
        [SwaggerOperation(
            Summary = LessonPodMessage.AllDistributedLessonodsByTeacherByLessonPodSummary,
            Description = LessonPodMessage.AllDistributedLessonodsByTeacherByLessonPodDescription
        )]
        [ProducesResponseType(typeof(AllDistributedLessonpodsByTeacherResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllDistributedLessonodsByTeacherByLessonPod([Required][FromHeader] int LoginID, [Required][FromHeader] int LessonUnitID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID + ", " + ControllerConstant.LessonUnitId + LessonUnitID
            });

            var response = await lessonPodService.AllDistributedLessonpodsByTeacherAsync(LoginID, LessonUnitID);

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

        #region GetBPSyncCourseDownload
        [Authorize(Roles = ControllerConstant.StudentRole)]
        [Route("GetBPSyncCourseDownload")]
        [OperationOrder(51)]
        [HttpGet]
        [SwaggerOperation(
            Summary = LessonPodMessage.SyncCourseDownloadSummary,
            Description = LessonPodMessage.SyncCourseDownloadDescription
        )]
        [ProducesResponseType(typeof(SyncCourseDownloadResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetBPSyncCourseDownload([Required][FromHeader] int StudentID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = StudentID,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.StudentId + StudentID
            });

            var response = await syncLessonPodService.SyncCourseDownloadAsync(StudentID);

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

        #region GetBPSyncLessonUnitDownload
        [Authorize(Roles = ControllerConstant.StudentRole)]
        [Route("GetBPSyncLessonUnitDownload")]
        [OperationOrder(52)]
        [HttpGet]
        [SwaggerOperation(
            Summary = LessonPodMessage.SyncLessonPodDownloadSummary,
            Description = LessonPodMessage.SyncLessonPodDownloadDescription
        )]
        [ProducesResponseType(typeof(SyncLessonUnitDownloadResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetBPSyncLessonUnitDownload([Required][FromHeader] int StudentID, [Required][FromHeader] int LessonUnitDistID, [FromHeader] int PreviousLessonUnitDistID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = StudentID,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.StudentId + StudentID + ", " + ControllerConstant.LessonUnitDistId + LessonUnitDistID + ", PreviousLessonUnitDistID: " + PreviousLessonUnitDistID
            });

            var response = await syncLessonPodService.SyncLessonUnitDownloadAsync(StudentID, LessonUnitDistID, PreviousLessonUnitDistID);

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

        #region GetBPSyncCourseLessonUnitDownload
        [Authorize(Roles = ControllerConstant.StudentRole)]
        [Route("GetBPSyncCourseLessonUnitDownload")]
        [OperationOrder(53)]
        [HttpGet]
        [SwaggerOperation(
            Summary = LessonPodMessage.SyncCourseLessonPodDownloadSummary,
            Description = LessonPodMessage.SyncCourseLessonPodDownloadDescription
        )]
        [ProducesResponseType(typeof(SyncCourseLessonUnitDownloadResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetBPSyncCourseLessonUnitDownload([Required][FromHeader] int StudentID, [Required][FromHeader] string CourseIDs)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = StudentID,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.StudentId + StudentID + ", CourseIDs: " + CourseIDs
            });

            var response = await syncLessonPodService.SyncCourseLessonUnitDownloadAsync(StudentID, CourseIDs);

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

        #region GetDistributeLessonUnit
        [Authorize(Roles = ControllerConstant.TeacherRole + "," + ControllerConstant.CurriculumAdminRole)]
        [Route("GetDistributeLessonUnit")]
        [OperationOrder(72)]
        [HttpGet]
        [SwaggerOperation(
            Summary = LessonPodMessage.DistributeLessonPodSummary,
            Description = LessonPodMessage.DistributeLessonPodDescription
        )]
        [ProducesResponseType(typeof(DistributeLessonPodResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDistributeLessonUnit([Required][FromHeader] int LessonUnitID, [Required][FromHeader] string LessonType)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LessonUnitId + LessonUnitID + ", LessonType: " + LessonType
            });

            var response = await lessonPodService.DistributeLessonPodAsync(LessonUnitID, LessonType);

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

        #region GetDistributedLessonUnit
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetDistributedLessonUnit")]
        [OperationOrder(73)]
        [HttpGet]
        [SwaggerOperation(
            Summary = LessonPodMessage.DistributedLessonPodSummary,
            Description = LessonPodMessage.DistributedLessonPodDescription
        )]
        [ProducesResponseType(typeof(DistributedLessonPodResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDistributedLessonUnit([Required][FromHeader] int LessonUnitDistID, [FromHeader] int LoginID)
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

            var response = await lessonPodService.DistributedLessonPodAsync(LessonUnitDistID);

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

        #region GetAllMyLessonPodsByLesson
        [Authorize(Roles = ControllerConstant.TeacherRole + "," + ControllerConstant.CurriculumAdminRole)]
        [Route("GetAllMyLessonPodsByLesson")]
        [OperationOrder(73)]
        [HttpGet]
        [SwaggerOperation(
            Summary = LessonPodMessage.DistributeLessonPodSummary,
            Description = LessonPodMessage.DistributeLessonPodDescription
        )]
        [ProducesResponseType(typeof(AllMyLessonPodsByLessonResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllMyLessonPodsByLesson([Required][FromHeader] int LessonID, [Required][FromHeader] int LoginID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID + ", " + ControllerConstant.LessonId + LessonID
            });

            var response = await lessonPodService.AllMyLessonPodsByLessonAsync(LessonID, LoginID);

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

        #region GetCRLessonUnitDetails
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetCRLessonUnitDetails")]
        [OperationOrder(74)]
        [HttpGet]
        [SwaggerOperation(
            Summary = LessonPodMessage.CRLessonUnitDetailsSummary,
            Description = LessonPodMessage.CRLessonUnitDetailsDescription
        )]
        [ProducesResponseType(typeof(CRLessonUnitDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCRLessonUnitDetails([Required][FromHeader] int LessonUnitDistID, [FromHeader] int LoginID)
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

            var response = await lessonPodService.CRLessonUnitDetailsAsync(LessonUnitDistID);

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

        #region GetLessonUnit
        [Authorize(Roles = ControllerConstant.TeacherRole + "," + ControllerConstant.CurriculumAdminRole)]
        [Route("GetLessonUnit")]
        [OperationOrder(75)]
        [HttpGet]
        [SwaggerOperation(
            Summary = LessonPodMessage.GetLessonPodSummary,
            Description = LessonPodMessage.GetLessonPodDescription
        )]
        [ProducesResponseType(typeof(LessonPodResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetLessonUnit([Required][FromHeader] int LessonUnitID, [Required][FromHeader] int AuthorID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = AuthorID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + AuthorID + ", " + ControllerConstant.LessonUnitId + LessonUnitID
            });

            var response = await lessonPodService.LessonPodAsync(LessonUnitID, AuthorID);

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

        #region GetAllMyLessonUnitByLesson
        [Authorize(Roles = ControllerConstant.TeacherRole + "," + ControllerConstant.CurriculumAdminRole)]
        [Route("GetAllMyLessonUnitByLesson")]
        [OperationOrder(76)]
        [HttpGet]
        [SwaggerOperation(
            Summary = LessonPodMessage.AllMyLessonPodsSummary,
            Description = LessonPodMessage.AllMyLessonPodsDescription
        )]
        [ProducesResponseType(typeof(AllMyLessonPodsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllMyLessonUnitByLesson([Required][FromHeader] int LessonID, [Required][FromHeader] int AuthorID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = AuthorID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + AuthorID + ", " + ControllerConstant.LessonId + LessonID
            });

            var response = await lessonPodService.AllMyLessonPodsAsync(LessonID, AuthorID);

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

        #region GetAllActivitiesByLessonUnit
        [Authorize(Roles = ControllerConstant.TeacherRole + "," + ControllerConstant.CurriculumAdminRole)]
        [Route("GetAllActivitiesByLessonUnit")]
        [OperationOrder(77)]
        [HttpGet]
        [SwaggerOperation(
            Summary = LessonPodMessage.AllActivitiesByLessonPodSummary,
            Description = LessonPodMessage.AllActivitiesByLessonPodDescription
        )]
        [ProducesResponseType(typeof(AllActivitiesByLessonPodResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllActivitiesByLessonUnit([Required][FromHeader] int LessonUnitID, [Required][FromHeader] int LoginID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID + ", " + ControllerConstant.LessonUnitId + LessonUnitID
            });

            var response = await lessonPodService.AllActivitiesByLessonPodAsync(LessonUnitID, LoginID);

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

        #region GetAllStudioSlideTemplates
        [Authorize(Roles = ControllerConstant.TeacherRole + "," + ControllerConstant.CurriculumAdminRole)]
        [Route("GetAllStudioSlideTemplates")]
        [OperationOrder(78)]
        [HttpGet]
        [SwaggerOperation(
            Summary = LessonPodMessage.AllSlideTemplateSummary,
            Description = LessonPodMessage.AllSlideTemplateDescription
        )]
        [ProducesResponseType(typeof(AllSlideTemplateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllStudioSlideTemplates([Required][FromHeader] int LoginID, [Required][FromHeader] int DomainID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.ClassLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID + ", DomainID: " + DomainID
            });

            var response = await lessonPodService.AllSlideTemplateAsync(DomainID, LoginID);

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
                
        #region GetLPStudioPreviewActivityBySlide
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetLPStudioPreviewActivityBySlide")]
        [OperationOrder(76)]
        [HttpGet]
        [SwaggerOperation(
            Summary = LessonPodMessage.SyncLessonPodDownloadSummary,
            Description = LessonPodMessage.SyncLessonPodDownloadDescription
        )]
        [ProducesResponseType(typeof(StudioPreviewActivityBySlideResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetLPStudioPreviewActivityBySlide([Required][FromHeader] int LoginID, [FromHeader] int LessonUnitID, [Required][FromHeader] string SlideID, [Required][FromHeader] string PreviewMode)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID + ", " + ControllerConstant.LessonUnitId + LessonUnitID + ", SlideID: " + SlideID + ", PreviewMode: " + PreviewMode
            });

            var response = await lessonPodService.GetLPStudioPreviewActivityBySlideAsync(LoginID, LessonUnitID, SlideID, PreviewMode);

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

        #region GetLPStudioPreviewActivitiesByLessonPod
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("GetLPStudioPreviewActivitiesByLessonPod")]
        [OperationOrder(77)]
        [HttpGet]
        [SwaggerOperation(
            Summary = LessonPodMessage.SyncLessonPodDownloadSummary,
            Description = LessonPodMessage.SyncLessonPodDownloadDescription
        )]
        [ProducesResponseType(typeof(StudioPreviewActivitiesByLessonPodResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetLPStudioPreviewActivitiesByLessonPod([Required][FromHeader] int LoginID, [FromHeader] int LessonUnitID, [Required][FromHeader] string PreviewMode)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = ControllerConstant.LoginId + LoginID + ", " + ControllerConstant.LessonUnitId + LessonUnitID + ", PreviewMode: " + PreviewMode
            });

            var response = await lessonPodService.GetLPStudioPreviewActivitiesByLessonPodAsync(LoginID, LessonUnitID, PreviewMode);

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
