using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Dependency.Library.Swagger;
using BackPack.Library.Constants;
using BackPack.Library.Messages;
using BackPack.Library.Requests.Activity;
using BackPack.Library.Requests.DBLog;
using BackPack.Library.Responses.Activity;
using BackPack.Library.Services.Interfaces.Activity;
using BackPack.Library.Services.Interfaces.DBLog;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace BackPack.WebAPI.Controllers.Activity
{
    [Authorize]
    [Route(ControllerConstant.ServiceEndPoint)]
    [ApiController]
    [ApiExplorerSettings(GroupName = ControllerConstant.Activity)]
    public class ActivityController(
        IDBLogService dbLogService, 
        IValidator<StudentActivityRequest> studentActivityValidator,
        IValidator<TeacherFeedbackOnActivityRequest> feedbackOnActivityValidator, 
        IActivityService activityService
        ) : BaseController(dbLogService)
    {
        #region SaveBackpackActivityForStudent
        [Authorize(Roles = ControllerConstant.StudentRole + "," + ControllerConstant.TeacherRole)]
        [Route("SaveBackpackActivityForStudent")]
        [OperationOrder(1)]
        [HttpPost]
        [SwaggerOperation(
            Summary = ActivityMessage.SaveActivitySummary,
            Description = ActivityMessage.SaveActivityDescription
        )]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]        

        public async Task<IActionResult> SaveActivity(StudentActivityRequest request)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = request.StudentID,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Post,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = JsonSerializer.Serialize(request)
            });
             
            #region Validation
            var validation = await studentActivityValidator.ValidateAsync(request);
            if (!validation.IsValid && validation.Errors.Any())
            {
                var validationResponse = await ValidationResponse(validation, guid);
                return new BadRequestObjectResult(validationResponse);
            }
            #endregion
            
            BaseResponse response = await activityService.SaveStudentActivityAsync(request);
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
                Success = false,
                StatusCode = StatusCodes.Status500InternalServerError,
                StatusMessage = CommonMessage.InternalServerErrorMessage
            });
            #endregion
        }
        #endregion

        #region SaveTeacherFeedbackOnActivity 
        [Authorize(Roles = ControllerConstant.TeacherRole)]
        [Route("SaveTeacherFeedbackOnActivity")]
        [OperationOrder(2)]
        [HttpPost]
        [SwaggerOperation(
            Summary = ActivityMessage.SaveTeacherFeedbackSummary,
            Description = ActivityMessage.SaveTeacherFeedbackDescription
        )]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(InternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveTeacherFeedbackOnActivity(TeacherFeedbackOnActivityRequest request)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = (int)request.AuthorID!,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Post,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = JsonSerializer.Serialize(request)
            });

            #region Validation
            var validation = await feedbackOnActivityValidator.ValidateAsync(request);
            if (!validation.IsValid && validation.Errors.Any())
            {
                var validationResponse = await ValidationResponse(validation, guid);
                return new BadRequestObjectResult(validationResponse);
            }
            #endregion

            BaseResponse response = await activityService.SaveTeacherFeedbackOnActivityAsync(request);

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
                Success = false,
                StatusCode = StatusCodes.Status500InternalServerError,
                StatusMessage = CommonMessage.InternalServerErrorMessage
            });
            #endregion
        }
        #endregion

        #region GetBackpackActivityForStudent
        [Authorize(Roles = ControllerConstant.StudentRole + "," + ControllerConstant.TeacherRole)]
        [Route("GetBackpackActivityForStudent")]
        [OperationOrder(21)]
        [HttpGet]
        [SwaggerOperation(
            Summary = ActivityMessage.BackpackActivityForStudentSummary,
            Description = ActivityMessage.BackpackActivityForStudentDescription
        )]
        [ProducesResponseType(typeof(BackpackActivityForStudentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetBackpackActivityForStudent([Required][FromHeader] int StudentID, [Required][FromHeader] int ContentID, [FromHeader] int ParentID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = StudentID,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "LoginID: " + StudentID + ", ContentID: " + ContentID + ", ParentID: " + ParentID
            });

            var response = await activityService.BackpackActivityForStudentAsync(StudentID, ContentID, ParentID);

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
