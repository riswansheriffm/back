
using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.MessageContract.Library;
using BackPack.Tenant.Library.Repositories.Interfaces;
using BackPack.Tenant.Library.Requests;
using BackPack.Tenant.Library.Responses;
using BackPack.Tenant.Library.Services.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BackPack.Tenant.Library.Services.Services
{
    public class CreateCourseCapsuleLicenseService(
        IGetTenantByDomainIdRepository getTenantByDomainIdRepository,
        IRequestClient<CreateCourseCapsuleLicenseEvent> requestClient,
        IConfiguration configuration
        ) : ICreateCourseCapsuleLicenseService
    {
        #region MyRegion
        public async Task<BaseResponse> CreateCourseCapsuleLicenseAsync(CreateCourseCapsuleLicenseRequest request)
        {
            var transitUrl = configuration.GetSection("CommonSettings").GetSection("TransactionUrl").Value;
            bool isActive = await GlobalHelper.ActivateTransitAsync(transitUrl!);
            if (!isActive)
            {
                return new BaseResponse()
                {
                    MessageID = CommonMessage.ErrorID,
                    Success = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = CommonMessage.InternalServerErrorMessage
                };
            }

            GetTenantByTenantNameResponse dbResponse = await getTenantByDomainIdRepository.GetTenantDBConnection(domainId: request.DomainID);

            if (dbResponse.Data.DBConnection != "")
            {
                try
                {
                    Response<ConsumerBaseResponse> consumerResponse = await requestClient!.GetResponse<ConsumerBaseResponse>(new CreateCourseCapsuleLicenseEvent()
                    {
                        DomainID = request.DomainID,
                        LoginID = request.LoginID,
                        DBConnection = dbResponse.Data.DBConnection,
                        CourseCapsuleID = request.CourseCapsuleID,
                        CourseID = request.CourseID,
                        StudentIDs = request.StudentIDs,
                        Duration = request.Duration,
                        WhomToLicense = request.WhomToLicense,
                        StartDate = request.StartDate,
                        LicenseAction = request.LicenseAction,
                        LicenseType = request.LicenseType,
                        ActivityBy = GlobalApplicationProperty.UserID
                    }, timeout: RequestTimeout.After(s: 60));

                    return new BaseResponse()
                    {
                        Success = consumerResponse.Message.Success,
                        StatusCode = consumerResponse.Message.StatusCode,
                        StatusMessage = consumerResponse.Message.StatusMessage,
                        ExceptionType = consumerResponse.Message.ExceptionType,
                        ExceptionMessage = consumerResponse.Message.ExceptionMessage,
                    };
                }
                catch (Exception)
                {
                    return new BaseResponse()
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = CommonMessage.BadRequestMessage
                    };
                }
            }
            return new BaseResponse()
            {
                Success = false,
                StatusCode = StatusCodes.Status400BadRequest,
                StatusMessage = CommonMessage.BadRequestMessage
            };
        }
        #endregion
    }
}
