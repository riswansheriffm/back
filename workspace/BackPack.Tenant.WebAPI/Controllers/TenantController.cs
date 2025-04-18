using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Dependency.Library.Swagger;
using BackPack.MessageContract.Library;
using BackPack.Tenant.Library.Constants;
using BackPack.Tenant.Library.Messages;
using BackPack.Tenant.Library.Requests;
using BackPack.Tenant.Library.Responses;
using BackPack.Tenant.Library.Services.Interfaces;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace BackPack.Tenant.WebAPI.Controllers
{
    [Authorize]
    [Route(ControllerConstant.ServiceEndPoint)]
    [ApiController]
    [ApiExplorerSettings(GroupName = ControllerConstant.Tenant)]
    public class TenantController(
        IDBLogService dbLogService, 
        IValidator<CreateTenantRequest> createTenantValidator, 
        IValidator<UpdateTenantRequest> updateTenantValidator,
        IValidator<DeleteTenantRequest> deleteTenantValidator,
        ITenantService tenantService,
        ICreateTenantService createTenantService,
        IGetTenantService getTenantService
        ) : BaseController(dbLogService)
    {
        #region CreateTenant
        [Route("CreateTenant")]
        [OperationOrder(01)]
        [HttpPost]
        [SwaggerOperation(
            Summary = TenantMessage.CreateTenantSummary,
            Description = TenantMessage.CreateTenantDescription
        )]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ConflictResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(InternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTenant(CreateTenantRequest request, [FromServices] IRequestClient<CreateDomainEvent> requestClient)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = request.ActivityBy,
                LoginType = LogConstant.SuperAdminLoginType,
                ServiceType = LogConstant.Post,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = JsonSerializer.Serialize(request)
            });

            #region Validation
            var validation = await createTenantValidator.ValidateAsync(request);
            if (!validation.IsValid && validation.Errors.Count > 0)
            {
                var validationResponse = await ValidationResponse(validation, guid);
                return new BadRequestObjectResult(validationResponse);
            }
            #endregion

            BaseResponse response = await createTenantService.CreateTenantAsync(request);

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

            if (response.StatusCode == StatusCodes.Status409Conflict)
            {
                return StatusCode(StatusCodes.Status409Conflict, new BadRequestResponse
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

        #region UpdateTenant
        [Route("UpdateTenant")]
        [OperationOrder(02)]
        [HttpPost]
        [SwaggerOperation(
            Summary = TenantMessage.UpdateTenantSummary,
            Description = TenantMessage.UpdateTenantDescription
        )]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ConflictResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(InternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTenant(UpdateTenantRequest request, [FromServices] IRequestClient<UpdateDomainEvent> requestClient)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = request.ActivityBy,
                LoginType = LogConstant.SuperAdminLoginType,
                ServiceType = LogConstant.Post,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = JsonSerializer.Serialize(request)
            });

            #region Validation
            var validation = await updateTenantValidator.ValidateAsync(request);
            if (!validation.IsValid && validation.Errors.Count > 0)
            {
                var validationResponse = await ValidationResponse(validation, guid);
                return new BadRequestObjectResult(validationResponse);
            }
            #endregion

            BaseResponse response = await tenantService.UpdateTenantAsync(request);

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
                response.StatusMessage = TenantMessage.UpdateTenantSuccess;
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

        #region DeleteTenant
        [Route("DeleteTenant")]
        [OperationOrder(03)]
        [HttpPost]
        [SwaggerOperation(
            Summary = TenantMessage.DeleteTenantSummary,
            Description = TenantMessage.DeleteTenantDescription
        )]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ConflictResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(InternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTenant(DeleteTenantRequest request, [FromServices] IRequestClient<DeleteDomainEvent> requestClient)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = request.ActivityBy,
                LoginType = LogConstant.SuperAdminLoginType,
                ServiceType = LogConstant.Post,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = JsonSerializer.Serialize(request)
            });

            #region Validation
            var validation = await deleteTenantValidator.ValidateAsync(request);
            if (!validation.IsValid && validation.Errors.Count > 0)
            {
                var validationResponse = await ValidationResponse(validation, guid);
                return new BadRequestObjectResult(validationResponse);
            }
            #endregion

            BaseResponse response = await tenantService.DeleteTenantAsync(request);

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
                response.StatusMessage = TenantMessage.DeleteTenantSuccess;
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

        #region GetTenant
        [Route("GetTenant")]
        [OperationOrder(04)]
        [HttpGet]
        [SwaggerOperation(
            Summary = TenantMessage.GetTenantSummary,
            Description = TenantMessage.GetTenantDescription
        )]
        [ProducesResponseType(typeof(GetDomainAcceptedEvent), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDistrict([Required][FromHeader] int DistrictID, [Required][FromHeader] Guid TenantID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = 0,
                LoginType = LogConstant.SuperAdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "DistrictID: " + DistrictID + ", TenantID: " + TenantID
            });

            var response = await getTenantService.GetTenantAsync(domainID: DistrictID, tenantID: TenantID);

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

        #region GetAllTenants
        [Route("GetAllTenants")]
        [OperationOrder(05)]
        [HttpGet]
        [SwaggerOperation(
            Summary = TenantMessage.GetAllTenantSummary,
            Description = TenantMessage.GetAllTenantDescription
        )]
        [ProducesResponseType(typeof(GetAllDistrictResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllTenants()
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginType = LogConstant.SuperAdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
            });

            var response = await getTenantService.GetAllTenantsAsync();

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
        
        #region GetAllActiveTenants
        [Route("GetAllActiveTenants")]
        [OperationOrder(08)]
        [HttpGet]
        [SwaggerOperation(
            Summary = TenantMessage.GetAllActiveTenantsSummary,
            Description = TenantMessage.GetAllActiveTenantsDescription
        )]
        [ProducesResponseType(typeof(GetAllActiveDomainsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllActiveTenants()
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginType = LogConstant.SuperAdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
            });

            var response = await getTenantService.GetAllActiveTenantsAsync();

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
