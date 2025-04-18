
using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Messages;
using BackPack.Dependency.Library.Responses;
using BackPack.MessageContract.Library;
using BackPack.Tenant.Library.Repositories.Interfaces;
using BackPack.Tenant.Library.Requests;
using BackPack.Tenant.Library.Services.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BackPack.Tenant.Library.Services.Services
{
    public class CreateTenantService(
        ICreateTenantRepository createTenantRepository,
        ITenantConsumerRepository tenantConsumerRepository,
        IRequestClient<CreateDomainEvent> requestClient,
        IConfiguration configuration
        ) : ICreateTenantService
    {
        #region CreateTenantAsync
        public async Task<BaseResponse> CreateTenantAsync(CreateTenantRequest request)
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

            request.TenantID = Guid.NewGuid();
            var exceptionMessage = "";
            string databaseConnection = "Server=";
            databaseConnection += request.DBServer!.Trim();
            if (!string.IsNullOrEmpty(request.DBPort)) databaseConnection += "," + request.DBPort;
            databaseConnection += ";Database=" + request.DBName + ";User Id=" + request.DBUser + ";Password=" + request.DBPassword + ";Pooling=false;";
            Aes256Helper aes256Helper = new(configuration!);
            request.DBConnection = aes256Helper.Aes256Encryption(text: databaseConnection);

            BaseResponse response = await createTenantRepository.CreateTenantAsync(request: request);

            if (!response.Success)
            {
                return new BaseResponse()
                {
                    Success = response.Success,
                    StatusCode = response.StatusCode,
                    StatusMessage = response.StatusMessage,
                    ExceptionMessage = response.ExceptionMessage,
                    ExceptionType = response.ExceptionType,
                };
            }

            try
            {
                Response<CreateDomainAcceptedEvent> consumerResponse = await requestClient!.GetResponse<CreateDomainAcceptedEvent>(new CreateDomainEvent()
                {
                    TenantID = request.TenantID,
                    DBConnection = request.DBConnection,
                    Country = request.Country!,
                    Name = request.Name!,
                    Desc = request.Desc!,
                    StreetAddress = request.StreetAddress!,
                    City = request.City!,
                    State = request.State!,
                    ZipCode = request.ZipCode!,
                    MaxStudents = request.MaxStudents!,
                    MaxTeachers = request.MaxTeachers!,
                    ActivityBy = GlobalApplicationProperty.UserID!,
                    AccessType = request.AccessType!,
                    EmailID = request.EmailID!,
                    FirstName = request.FirstName!,
                    LastName = request.LastName!,
                    LoginName = request.LoginName!,
                    PhoneNo = request.PhoneNo!,
                    AccessToken = request.AccessToken!,
                    ApplicationID = request.ApplicationID!,
                    SourceID = request.SourceID!
                }, timeout: RequestTimeout.After(s: 60));

                if (!consumerResponse.Message.Success)
                {
                    await tenantConsumerRepository.DeleteTenantByConsumerRejectedAsync(tenantID: request.TenantID);

                    return new BaseResponse()
                    {
                        Success = consumerResponse.Message.Success,
                        StatusCode = consumerResponse.Message.StatusCode,
                        StatusMessage = consumerResponse.Message.StatusMessage,
                    };
                }

                await tenantConsumerRepository.UpdateTenantByConsumerAcceptedAsync(tenantID: request.TenantID, domainID: consumerResponse.Message.DomainID);

                return new BaseResponse()
                {
                    Success = response.Success,
                    StatusCode = response.StatusCode,
                    StatusMessage = response.StatusMessage,
                };
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
                await tenantConsumerRepository.DeleteTenantByConsumerRejectedAsync(tenantID: request.TenantID);
            }

            return new BaseResponse()
            {
                MessageID = CommonMessage.ErrorID,
                Success = false,
                StatusCode = StatusCodes.Status500InternalServerError,
                StatusMessage = CommonMessage.InternalServerErrorMessage,
                ExceptionMessage = exceptionMessage,
                ExceptionType = CommonMessage.ExceptionTypeFail
            };
        }
        #endregion
    }
}
