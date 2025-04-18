using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Dependency.Library.Swagger;
using BackPack.Library.Constants;
using BackPack.Library.Requests.DBLog;
using BackPack.Library.Requests.User;
using BackPack.Library.Responses.User;
using BackPack.Library.Services.Interfaces.DBLog;
using BackPack.Library.Services.Interfaces.User;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using BackPack.Library.Messages;
using Microsoft.AspNetCore.Authorization;
using BackPack.Library.Requests.Token;
using System.ComponentModel.DataAnnotations;

namespace BackPack.WebAPI.Controllers.User
{
    [Route(ControllerConstant.ServiceEndPoint)]
    [ApiController]
    [ApiExplorerSettings(GroupName = ControllerConstant.User)]
    public class UserController(
        IDBLogService dbLogService, 
        IValidator<LoginRequest> loginValidator,
        IValidator<ResetPasswordRequest> resetPasswordValidator,
        IValidator<RefreshTokenRequest> refreshTokenValidator,
        IUserService userService,
        ILoginService loginService,
        ITokenService tokenService
        ) : BaseController(dbLogService)
    {

        #region TeacherLogin
        [Route("TeacherLogin")]
        [OperationOrder(1)]
        [HttpPost]
        [SwaggerOperation(
            Summary = UserMessage.TeacherLoginSummary,
            Description = UserMessage.TeacherLoginDescription
        )]
        [ProducesResponseType(typeof(UserLoginResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> TeacherLogin(LoginRequest request)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = 0,
                LoginType = LogConstant.AdminLoginType,
                ServiceType = LogConstant.Post,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "LoginName: " + request.LoginName + ", DistrictName: " + request.DistrictName
            });

            #region Validation            
            var validation = await loginValidator.ValidateAsync(request);
            if (!validation.IsValid && validation.Errors.Any())
            {
                var validationResponse = await ValidationResponse(validation, guid);
                return new BadRequestObjectResult(validationResponse);
            }
            #endregion

            var response = await loginService.CreateUserTokenAsync(request, ServiceConstant.Teacher);

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

        #region StudentLogin
        [Route("StudentLogin")]
        [OperationOrder(2)]
        [HttpPost]
        [SwaggerOperation(
            Summary = UserMessage.StudentLoginSummary,
            Description = UserMessage.StudentLoginDescription
        )]
        [ProducesResponseType(typeof(UserLoginResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> StudentLogin(LoginRequest request)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = 0,
                LoginType = LogConstant.AppLoginType,
                ServiceType = LogConstant.Post,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "LoginName: " + request.LoginName + ", DistrictName: " + request.DistrictName
            });

            #region Validation            
            var validation = await loginValidator.ValidateAsync(request);
            if (!validation.IsValid && validation.Errors.Any())
            {
                var validationResponse = await ValidationResponse(validation, guid);
                return new BadRequestObjectResult(validationResponse);
            }
            #endregion

            var response = await loginService.CreateUserTokenAsync(request, ServiceConstant.Student);

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

        #region ActivateUserAccount
        [Route("ActivateUserAccount")]
        [OperationOrder(3)]
        [HttpPost]
        [SwaggerOperation(
            Summary = UserMessage.ActivateUserAccountSummary,
            Description = UserMessage.ActivateUserAccountDescription
        )]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActivateUserAccount(ResetPasswordRequest request)
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
            var validation = await resetPasswordValidator.ValidateAsync(request);
            if (!validation.IsValid && validation.Errors.Any())
            {
                var validationResponse = await ValidationResponse(validation, guid);
                return new BadRequestObjectResult(validationResponse);
            }
            #endregion

            BaseResponse response = await userService.ActivateUserAccountAsync(request);

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
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new InternalServerErrorResponse
                {
                    MessageID = response.MessageID,
                    Success = response.Success,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = CommonMessage.InternalServerErrorMessage
                });
            }
            #endregion
        }
        #endregion

        #region ResetPassword
        [Route("ResetPassword")]
        [OperationOrder(3)]
        [HttpPost]
        [SwaggerOperation(
            Summary = UserMessage.ResetCredentialSummary,
            Description = UserMessage.ResetCredentialDescription
        )]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
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
            var validation = await resetPasswordValidator.ValidateAsync(request);
            if (!validation.IsValid && validation.Errors.Any())
            {
                var validationResponse = await ValidationResponse(validation, guid);
                return new BadRequestObjectResult(validationResponse);
            }
            #endregion

            BaseResponse response = await userService.ResetPasswordAsync(request);

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
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new InternalServerErrorResponse
                {
                    MessageID = response.MessageID,
                    Success = response.Success,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = CommonMessage.InternalServerErrorMessage
                });
            }
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
                LoginType = LogConstant.AppLoginType,
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

            var response = await loginService.CreateRefreshTokenAsync(request.UserID, request.UserType!.ToString(), refreshTokenResponse.ResultCount, refreshTokenResponse.DomainName!);
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

        #region GetUser
        [Authorize]
        [Authorize(Roles = ControllerConstant.DistrictAdminRole + "," + ControllerConstant.CurriculumAdminRole + "," + ControllerConstant.SchoolAdminRole)]
        [Route("GetUser")]
        [OperationOrder(04)]
        [HttpGet]
        [SwaggerOperation(
            Summary = UserMessage.UserSummary,
            Description = UserMessage.UserDescription
        )]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUser([Required][FromHeader] int LoginID)
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

            var response = await userService.UserAsync(LoginID);

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

        #region GetAllUser
        [Authorize]
        [Authorize(Roles = ControllerConstant.DistrictAdminRole + "," + ControllerConstant.CurriculumAdminRole + "," + ControllerConstant.SchoolAdminRole)]
        [Route("GetAllUser")]
        [OperationOrder(05)]
        [HttpGet]
        [SwaggerOperation(
            Summary = UserMessage.AllUserSummary,
            Description = UserMessage.AllUserDescription
        )]
        [ProducesResponseType(typeof(AllUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllUser([Required][FromHeader] int DistrictID, [Required][FromHeader] int SchoolID, [Required][FromHeader] string RoleName)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = 0,
                LoginType = LogConstant.AdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "DomainID: " + DistrictID + ", SchoolID: " + SchoolID + ", RoleName: " + RoleName
            });

            var response = await userService.AllUsersAsync(DistrictID, SchoolID, RoleName);

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

        #region GetAllDomainUser
        [Authorize]
        [Authorize(Roles = ControllerConstant.DistrictAdminRole + "," + ControllerConstant.CurriculumAdminRole + "," + ControllerConstant.SchoolAdminRole)]
        [Route("GetAllDomainUser")]
        [OperationOrder(06)]
        [HttpGet]
        [SwaggerOperation(
            Summary = UserMessage.AllDomainUserSummary,
            Description = UserMessage.AllDomainUserDescription
        )]
        [ProducesResponseType(typeof(AllDomainUsersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllDomainUser([Required][FromHeader] int DistrictID, [FromHeader] int SchoolID, [Required][FromHeader] string Role)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = 0,
                LoginType = LogConstant.AdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "DomainID: " + DistrictID + ", SchoolID: " + SchoolID + ", Role: " + Role
            });

            var response = await userService.AllDomainUserAsync(DistrictID, SchoolID, Role);

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

        #region GetAllTeachersByClass
        [Authorize]
        [Authorize(Roles = ControllerConstant.CurriculumAdminRole)]
        [Route("GetAllTeachersByClass")]
        [OperationOrder(07)]
        [HttpGet]
        [SwaggerOperation(
            Summary = UserMessage.GetAllTeacherByClassSummary,
            Description = UserMessage.GetAllTeacherByClassDescription
        )]
        [ProducesResponseType(typeof(AllTeacherByClassResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllTeachersByClass([Required][FromHeader] int LoginID, [Required][FromHeader] int CourseID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = LoginID,
                LoginType = LogConstant.AdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "LoginID: " + LoginID + ", CourseID: " + CourseID
            });

            var response = await userService.AllTeacherByClassAsync(LoginID, CourseID);

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

        #region GetSuperCA
        [Authorize]
        [Authorize(Roles = ControllerConstant.DistrictAdminRole)]
        [Route("GetSuperCA")]
        [OperationOrder(06)]
        [HttpGet]
        [SwaggerOperation(
            Summary = UserMessage.SuperCaUserSummary,
            Description = UserMessage.SuperCaUserDescription
        )]
        [ProducesResponseType(typeof(SuperCaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSuperCA([Required][FromHeader] int DistrictID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = 0,
                LoginType = LogConstant.AdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "DomainID: " + DistrictID
            });

            var response = await userService.SuperCaAsync(DistrictID);

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
