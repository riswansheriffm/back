using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.Dependency.Library.Swagger;
using BackPack.Library.Constants;
using BackPack.Library.Messages;
using BackPack.Library.Requests.ClassLink;
using BackPack.Library.Requests.DBLog;
using BackPack.Library.Responses.ClassLink;
using BackPack.Library.Services.Interfaces.ClassLink;
using BackPack.Library.Services.Interfaces.DBLog;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace BackPack.WebAPI.Controllers.ClassLink
{
    [Authorize]
    [Route(ControllerConstant.ServiceEndPoint)]
    [ApiController]
    [ApiExplorerSettings(GroupName = ControllerConstant.ClassLink)]
    public class ClassLinkController(
        IDBLogService dbLogService,
        IClassLinkService classLinkService,
        IValidator<CreateClassLinkRequest> createClassLinkRequestValidator
        ) : BaseController(dbLogService)
    {
        #region ClassLinkData
        [Authorize(Roles = ControllerConstant.DistrictAdminRole)]
        [Route("ClassLinkData")]
        [OperationOrder(01)]
        [HttpPost]
        [SwaggerOperation(
             Summary = ClassLinkMessage.CreateClassLinkSummary,
             Description = ClassLinkMessage.CreateClassLinkDescription
        )]
        [ProducesResponseType(typeof(ClassLinkResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(InternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ClassLinkData(CreateClassLinkRequest request)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginType = LogConstant.AdminLoginType,
                ServiceType = LogConstant.Post,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = JsonSerializer.Serialize(request)
            });

            #region Validation
            var validation = await createClassLinkRequestValidator.ValidateAsync(request);
            if (!validation.IsValid && validation.Errors.Any())
            {
                var validationResponse = await ValidationResponse(validation, guid);
                return new BadRequestObjectResult(validationResponse);
            }
            #endregion

            ClassLinkResponse response = await classLinkService.ClassLinkAsync(request);

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
    }

}
