using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Responses;
using BackPack.Tenant.Library.Repositories.Interfaces;
using BackPack.Tenant.Library.Requests;
using BackPack.Tenant.Library.Services.Interfaces;
using MassTransit;
using BackPack.MessageContract.Library;
using BackPack.Tenant.Library.Responses;
using BackPack.Dependency.Library.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BackPack.Tenant.Library.Services.Services
{
    public class TenantService(        
        IUpdateTenantRepository updateTenantRepository,
        IDeleteTenantRepository deleteTenantRepository,        
        IRequestClient<UpdateDomainEvent> requestUpdateDomainClient,
        IRequestClient<DeleteDomainEvent> requestDeleteDomainClient,
        IGetTenantByTenantIDRepository getTenantByTenantIDRepository,
        IConfiguration configuration
        ) : ITenantService
    {
        #region UpdateTenantAsync
        public async Task<BaseResponse> UpdateTenantAsync(UpdateTenantRequest request)
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

            GetTenantByTenantNameResponse dbResponse = await getTenantByTenantIDRepository.GetTenantDBConnection(tenantID: request.TenantID);

            if (dbResponse.Data.DBConnection != "")
            {
                try
                {
                    Response<ConsumerBaseResponse> consumerResponse = await requestUpdateDomainClient!.GetResponse<ConsumerBaseResponse>(new UpdateDomainEvent()
                    {
                        TenantID = request.TenantID,
                        DomainID = request.DomainID,
                        DBConnection = dbResponse.Data.DBConnection,
                        Name = request.Name!,
                        Desc = request.Desc!,
                        StreetAddress = request.StreetAddress!,
                        City = request.City!,
                        State = request.State!,
                        ZipCode = request.ZipCode!,
                        MaxStudents = request.MaxStudents!,
                        MaxTeachers = request.MaxTeachers!,
                        ActivityBy = GlobalApplicationProperty.UserID,
                        EmailID = request.EmailID!,
                        FirstName = request.FirstName!,
                        LastName = request.LastName!,
                        LoginName = request.LoginName!,
                        PhoneNo = request.PhoneNo!,
                        ApplicationID = request.ApplicationID!,
                        AccessToken = request.AccessToken!,
                        SourceID = request.SourceID!,
                    }, timeout: RequestTimeout.After(s: 60));

                    if (consumerResponse.Message.Success)
                    {
                        await updateTenantRepository.UpdateTenantAsync(request);
                    }

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

        #region DeleteTenantAsync
        public async Task<BaseResponse> DeleteTenantAsync(DeleteTenantRequest request)
        {
            GetTenantByTenantNameResponse dbResponse = await getTenantByTenantIDRepository.GetTenantDBConnection(tenantID: request.TenantID);

            if (dbResponse.Data.DBConnection != "")
            {
                try
                {
                    Response<ConsumerBaseResponse> consumerResponse = await requestDeleteDomainClient!.GetResponse<ConsumerBaseResponse>(new DeleteDomainEvent()
                    {
                        TenantID = request.TenantID,
                        DomainID = request.DomainID,
                        DBConnection = dbResponse.Data.DBConnection,
                        ActivityBy = GlobalApplicationProperty.UserID,
                    }, timeout: RequestTimeout.After(s: 60));

                    if (consumerResponse.Message.Success)
                    {
                        await deleteTenantRepository.DeleteTenantAsync(request);
                    }

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
