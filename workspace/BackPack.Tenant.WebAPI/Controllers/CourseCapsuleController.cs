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
    [ApiExplorerSettings(GroupName = ControllerConstant.CourseCapsule)]
    public class CourseCapsuleController(
        IDBLogService dbLogService,
        IValidator<CreateCourseCapsuleLicenseRequest> createCourseCapsuleLicenseRequestValidator,
        ICourseCapsuleService courseCapsuleService,
        ICreateCourseCapsuleLicenseService createCourseCapsuleLicenseService,
        IGetTenantService getTenantService
        ) : BaseController(dbLogService)
    {
        #region GetAllPublicActiveTenants
        [Route("GetAllPublicActiveTenants")]
        [OperationOrder(01)]
        [HttpGet]
        [SwaggerOperation(
           Summary = TenantMessage.GetAllPublicActiveTenantsSummary,
            Description = TenantMessage.GetAllPublicActiveTenantsDescription
        )]
        [ProducesResponseType(typeof(AllPublicActiveDomainsResultResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllPublicActiveDomains()
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginType = LogConstant.SuperAdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
            });

            var response = await getTenantService.GetAllPublicActiveTenantsAsync();

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

        #region GetAllSubjectsByDomain
        [Route("GetAllSubjectsByDomain")]
        [OperationOrder(02)]
        [HttpGet]
        [SwaggerOperation(
            Summary = CourseCapsuleMessage.PublicCourseCapsuleSummary,
            Description = CourseCapsuleMessage.PublicCourseCapsuleDescription
        )]
        [ProducesResponseType(typeof(PublicCourseCapsuleByDomainResponseEvent), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllSubjectsByDomain([FromHeader] int LoginID, [Required][FromHeader] int DomainID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = 0,
                LoginType = LogConstant.SuperAdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "LoginID" + LoginID + ", DomainID: " + DomainID
            });

            var response = await courseCapsuleService.GetAllSubjectsByDomainAsync(loginId: LoginID, domainId: DomainID);

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
        [OperationOrder(03)]
        [HttpGet]
        [SwaggerOperation(
            Summary = CourseCapsuleMessage.PublicCourseCapsuleSummary,
            Description = CourseCapsuleMessage.PublicCourseCapsuleDescription
        )]
        [ProducesResponseType(typeof(PublicCourseCapsuleByDomainResponseEvent), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllPublicCourseCapsuleByDomainAndSubject([FromHeader] int LoginID, [Required][FromHeader] int DomainID, [Required][FromHeader] int SubjectID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = 0,
                LoginType = LogConstant.SuperAdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "LoginID" + LoginID + ", DomainID: " + DomainID + ", SubjectID: " + SubjectID
            });

            var response = await courseCapsuleService.GetPublicCourseCapsuleByDomainAsync(loginId: LoginID, domainId: DomainID, subjectId: SubjectID);

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

        #region GetCourseCapsuleByCapsule
        [Route("GetCourseCapsuleByCapsule")]
        [OperationOrder(04)]
        [HttpGet]
        [SwaggerOperation(
            Summary = CourseCapsuleMessage.CourseCapsuleByCapsuleSummary,
            Description = CourseCapsuleMessage.CourseCapsuleByCapsuleDescription
        )]
        [ProducesResponseType(typeof(CourseCapsuleByCapsuleResponseEvent), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCourseCapsuleByCapsule([FromHeader] int LoginID, [Required][FromHeader] int DomainID, [Required][FromHeader] int CourseCapsuleID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = 0,
                LoginType = LogConstant.SuperAdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "LoginID" + LoginID + ", DomainID: " + DomainID + ", CourseCapsuleID: " + CourseCapsuleID
            });

            var response = await courseCapsuleService.GetCourseCapsuleByCapsuleAsync(courseCapsuleId: CourseCapsuleID, domainId: DomainID);

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

        #region GetAllCoursesByDomain
        [Route("GetAllCoursesByDomain")]
        [OperationOrder(05)]
        [HttpGet]
        [SwaggerOperation(
            Summary = CourseCapsuleMessage.AllCoursesByDomainSummary,
            Description = CourseCapsuleMessage.AllCoursesByDomainDescription
        )]
        [ProducesResponseType(typeof(AllCoursesByDomainResponseEvent), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllCoursesByDomain([FromHeader] int LoginID, [Required][FromHeader] int DomainID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = 0,
                LoginType = LogConstant.SuperAdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "LoginID" + LoginID + ", DomainID: " + DomainID
            });

            var response = await courseCapsuleService.GetAllCoursesByDomainAsync(domainId: DomainID);

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
        [Route("GetAllTeachersByClass")]
        [OperationOrder(06)]
        [HttpGet]
        [SwaggerOperation(
            Summary = CourseCapsuleMessage.AllCoursesByDomainSummary,
            Description = CourseCapsuleMessage.AllCoursesByDomainDescription
        )]
        [ProducesResponseType(typeof(AllCoursesByDomainResponseEvent), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllTeachersByClass([FromHeader] int LoginID, [Required][FromHeader] int DomainID, [Required][FromHeader] int CourseID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = 0,
                LoginType = LogConstant.SuperAdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "LoginID" + LoginID + ", DomainID: " + DomainID + ", CourseID: " + CourseID
            });

            var response = await courseCapsuleService.GetAllTeachersByClassAsync(domainId: DomainID, courseId: CourseID);

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
        [Route("GetAllLPCourseLicenses")]
        [OperationOrder(07)]
        [HttpGet]
        [SwaggerOperation(
            Summary = CourseCapsuleMessage.AllCoursesByDomainSummary,
            Description = CourseCapsuleMessage.AllCoursesByDomainDescription
        )]
        [ProducesResponseType(typeof(AllCoursesByDomainResponseEvent), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllLPCourseLicenses([FromHeader] int LoginID, [Required][FromHeader] int DomainID, [Required][FromHeader] int CourseID, [Required][FromHeader] int CourseCapsuleID)
        {
            Guid guid = await CreateDBLogAsync(new DBLogRequest
            {
                LoginID = 0,
                LoginType = LogConstant.SuperAdminLoginType,
                ServiceType = LogConstant.Get,
                ServiceName = ControllerContext.RouteData.Values[LogConstant.LogController]!.ToString() + LogConstant.Controller,
                ServiceMethodName = ControllerContext.RouteData.Values[LogConstant.LogAction]!.ToString()!,
                ServiceRequest = "LoginID" + LoginID + ", DomainID: " + DomainID + ", CourseID: " + CourseID + ", CourseCapsuleID: " + CourseCapsuleID
            });

            var response = await courseCapsuleService.GetAllLPCourseLicensesAsync(domainId: DomainID, courseId: CourseID, courseCapsuleId: CourseCapsuleID);

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

        #region CreateCourseCapsuleLicense
        [Route("CreateCourseCapsuleLicense")]
        [OperationOrder(07)]
        [HttpPost]
        [SwaggerOperation(
            Summary = CourseCapsuleMessage.CreateCourseCapsuleLicenseSummary,
            Description = CourseCapsuleMessage.CreateCourseCapsuleLicenseDescription
        )]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(InternalServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCourseCapsuleLicense(CreateCourseCapsuleLicenseRequest request)
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
            var validation = await createCourseCapsuleLicenseRequestValidator.ValidateAsync(request);
            if (!validation.IsValid && validation.Errors.Any())
            {
                var validationResponse = await ValidationResponse(validation, guid);
                return new BadRequestObjectResult(validationResponse);
            }
            #endregion

            BaseResponse response = await createCourseCapsuleLicenseService.CreateCourseCapsuleLicenseAsync(request);

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
