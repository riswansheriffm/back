using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Dependency.Library.Swagger;
using BackPack.Library.Constants;
using BackPack.Library.Messages;
using BackPack.Library.Requests.DBLog;
using BackPack.Library.Requests.User;
using BackPack.Library.Services.Interfaces.DBLog;
using BackPack.Library.Services.Interfaces.User;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace BackPack.WebAPI.Controllers.User
{
    [Authorize]
    [Route(ControllerConstant.ServiceEndPoint)]
    [ApiController]
    [ApiExplorerSettings(GroupName = ControllerConstant.User)]
    public class CreateUserController(
        IDBLogService dbLogService,
        ICreateUserService createUserService,
        IValidator<SuperUserRequest> userRequestValidator
        ) : BaseController(dbLogService)
    {
        #region CreateSuperUser
        [Authorize(Roles = ControllerConstant.DistrictAdminRole)]
        [Route(RouteConstant.RouteCreateSuperUser)]
        [OperationOrder(2)]
        [HttpPost]
        [SwaggerOperation(
            Summary = UserMessage.SuperUserCreateSummary,
            Description = UserMessage.SuperUserCreateDescription
        )]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(InternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSuperUser(SuperUserRequest request)
        {
            if (!IsValidDomainUser(request.DistrictID)) return new BadRequestObjectResult(ReturnBadRequestResponse());
            if (!IsValidUserRole(request.UserType!)) return new BadRequestObjectResult(ReturnBadRequestResponse());

            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = request.ActivityBy,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Post,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = JsonSerializer.Serialize(request)
            });

            #region Validation            
            var validation = await userRequestValidator.ValidateAsync(request);
            if (!validation.IsValid && validation.Errors.Count != 0)
            {
                var validationResponse = await ValidationResponse(validation, guid);
                return new BadRequestObjectResult(validationResponse);
            }
            #endregion

            BaseResponse response = await createUserService.CreateSuperUserAsync(request);

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
    }
}
