using BackPack.Library.Constants;
using BackPack.Library.Messages;
using BackPack.Library.Requests.DBLog;
using BackPack.Library.Requests.LessonPod;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Services.Interfaces.DBLog;
using BackPack.Library.Services.Interfaces.LessonPod;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using BackPack.Dependency.Library.Swagger;
using BackPack.Dependency.Library.Messages;

namespace BackPack.WebAPI.Controllers.LessonPod
{
    [Authorize]
    [Route(ControllerConstant.ServiceEndPoint)]
    [ApiController]
    [ApiExplorerSettings(GroupName = ControllerConstant.LessonPod)]
    public class UnlockOfflineLessonPodController(
        IDBLogService dbLogService,
        IValidator<UnlockOfflineLessonPodRequest> unlockOfflineLessonPodValidator,
        IValidator<UnlockOfflineLessonPodDistRequest> unlockOfflineLessonPodDistValidator,
        IUnlockOfflineLessonPodService unlockOfflineLessonPodService
        ) : BaseController(dbLogService)
    {
        #region UnlockOfflineLessonUnitByCourseID
        [Authorize(Roles = ControllerConstant.StudentRole)]
        [Route("UnlockOfflineLessonUnitByCourseID")]
        [OperationOrder(4)]
        [HttpPost]
        [SwaggerOperation(
            Summary = LessonPodMessage.UnlockOfflineLessonUnitByCourseIDSummary,
            Description = LessonPodMessage.UnlockOfflineLessonUnitByCourseIDDescription
        )]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(InternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UnlockOfflineLessonUnitByCourseID(UnlockOfflineLessonPodRequest request)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = request.LoginID,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Post,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = JsonSerializer.Serialize(request)
            });

            #region Validation
            var validation = await unlockOfflineLessonPodValidator.ValidateAsync(request);
            if (!validation.IsValid && validation.Errors.Any())
            {
                var validationResponse = await ValidationResponse(validation, guid);
                return new BadRequestObjectResult(validationResponse);
            }
            #endregion

            BaseResponse response = await unlockOfflineLessonPodService.UnlockOfflineLessonUnitByCourseIDAsync(request);
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
                    Success = response.Success,
                    StatusCode = response.StatusCode,
                    StatusMessage = response.StatusMessage
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new InternalServerErrorResponse
                {
                    Success = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = CommonMessage.InternalServerErrorMessage
                });
            }
            #endregion
        }
        #endregion

        #region UnlockOfflineLessonUnitByLessonUnitDistID
        [Authorize(Roles = ControllerConstant.StudentRole)]
        [Route("UnlockOfflineLessonUnitByLessonUnitDistID")]
        [OperationOrder(5)]
        [HttpPost]
        [SwaggerOperation(
            Summary = LessonPodMessage.UnlockOfflineLessonUnitByCourseIDSummary,
            Description = LessonPodMessage.UnlockOfflineLessonUnitByCourseIDDescription
        )]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(InternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UnlockOfflineLessonUnitByLessonUnitDistID(UnlockOfflineLessonPodDistRequest request)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = request.LoginID,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Post,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = JsonSerializer.Serialize(request)
            });

            #region Validation
            var validation = await unlockOfflineLessonPodDistValidator.ValidateAsync(request);
            if (!validation.IsValid && validation.Errors.Any())
            {
                var validationResponse = await ValidationResponse(validation, guid);
                return new BadRequestObjectResult(validationResponse);
            }
            #endregion

            BaseResponse response = await unlockOfflineLessonPodService.UnlockOfflineLessonUnitByDistIDAsync(request);
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
                    Success = response.Success,
                    StatusCode = response.StatusCode,
                    StatusMessage = response.StatusMessage
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new InternalServerErrorResponse
                {
                    Success = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = CommonMessage.InternalServerErrorMessage
                });
            }
            #endregion
        }
        #endregion

        #region UnlockOfflineLessonUnitByOfflineLessonUnitDistID
        [Authorize(Roles = ControllerConstant.StudentRole)]
        [Route("UnlockOfflineLessonUnitByOfflineLessonUnitDistID")]
        [OperationOrder(5)]
        [HttpPost]
        [SwaggerOperation(
            Summary = LessonPodMessage.UnlockOfflineLessonUnitByCourseIDSummary,
            Description = LessonPodMessage.UnlockOfflineLessonUnitByCourseIDDescription
        )]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(InternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UnlockOfflineLessonUnitByOfflineLessonUnitDistID(UnlockOfflineLessonPodDistRequest request)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = request.LoginID,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Post,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = JsonSerializer.Serialize(request)
            });

            #region Validation
            var validation = await unlockOfflineLessonPodDistValidator.ValidateAsync(request);
            if (!validation.IsValid && validation.Errors.Any())
            {
                var validationResponse = await ValidationResponse(validation, guid);
                return new BadRequestObjectResult(validationResponse);
            }
            #endregion

            BaseResponse response = await unlockOfflineLessonPodService.UnlockOfflineLessonUnitByOfflineLessonUnitDistIDAsync(request);
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
                    Success = response.Success,
                    StatusCode = response.StatusCode,
                    StatusMessage = response.StatusMessage
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new InternalServerErrorResponse
                {
                    Success = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = CommonMessage.InternalServerErrorMessage
                });
            }
            #endregion
        }
        #endregion
    }
}
