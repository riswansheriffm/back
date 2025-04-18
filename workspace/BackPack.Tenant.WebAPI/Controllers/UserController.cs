using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Dependency.Library.Swagger;
using BackPack.Tenant.Library.Constants;
using BackPack.Tenant.Library.Messages;
using BackPack.Tenant.Library.Requests;
using BackPack.Tenant.Library.Responses;
using BackPack.Tenant.Library.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace BackPack.Tenant.WebAPI.Controllers
{
    [Route(ControllerConstant.ServiceEndPoint)]
    [ApiController]
    [ApiExplorerSettings(GroupName = ControllerConstant.User)]
    public class UserController(
        IDBLogService dbLogService, 
        IValidator<LoginRequest> loginValidator,
        IValidator<RefreshTokenRequest> refreshTokenValidator,
        ILoginService loginService,
        ITokenService tokenService
        ) : BaseController(dbLogService)
    {
        #region SuperAdminLogin
        [Route("SuperAdminLogin")]
        [OperationOrder(2)]
        [HttpPost]
        [SwaggerOperation(
            Summary = UserMessage.SuperAdminLoginSummary,
            Description = UserMessage.SuperAdminLoginDescription
        )]
        [ProducesResponseType(typeof(UserLoginResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SuperAdminLogin(LoginRequest request)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = 0,
                LoginType = LogConstant.SuperAdminLoginType,
                ServiceType = LogConstant.Post,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = JsonSerializer.Serialize(request)
            });

            #region Validation            
            var validation = await loginValidator.ValidateAsync(request);
            if (!validation.IsValid && validation.Errors.Any())
            {
                var validationResponse = await ValidationResponse(validation, guid);
                return new BadRequestObjectResult(validationResponse);
            }
            #endregion

            var response = await loginService.CreateUserTokenAsync(request, ServiceConstant.SuperAdmin);

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

        #region RefreshToken
        [Route("RefreshToken")]
        [OperationOrder(2)]
        [HttpPost]
        [SwaggerOperation(
            Summary = UserMessage.RefreshTokenSummary,
            Description = UserMessage.RefreshTokenDescription
        )]
        [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = 0,
                LoginType = LogConstant.SuperAdminLoginType,
                ServiceType = LogConstant.Post,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = JsonSerializer.Serialize(request)
            });

            #region Validation
            var validation = await refreshTokenValidator.ValidateAsync(request);
            if (!validation.IsValid && validation.Errors.Any())
            {
                var validationResponse = await ValidationResponse(validation, guid);
                return new BadRequestObjectResult(validationResponse);
            }
            #endregion

            var refreshTokenResponse = await tokenService.ValidateRefreshTokenAsync(request);
            if (!refreshTokenResponse.Success)
            {
                await UpdateDBLogAsync(new DBLogRequest
                {
                    ServiceLogID = guid,
                    ServiceResponse = JsonSerializer.Serialize(refreshTokenResponse),
                    ServiceStatus = CommonMessage.ServiceStatusFail,
                    LogMessage = refreshTokenResponse.StatusMessage,
                    ExceptionType = CommonMessage.ExceptionTypeFail,
                    ExceptionMessage = refreshTokenResponse.ExceptionMessage,
                    Success = refreshTokenResponse.Success
                });

                return BadRequest(refreshTokenResponse);
            }

            var response = await loginService.CreateRefreshTokenAsync(request.UserID, request.UserType!.ToString(), refreshTokenResponse.DomainName!);
            
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
