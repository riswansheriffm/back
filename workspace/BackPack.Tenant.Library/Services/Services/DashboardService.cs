using BackPack.MessageContract.Library;
using BackPack.Tenant.Library.Repositories.Interfaces;
using BackPack.Tenant.Library.Services.Interfaces;
using BackPack.Tenant.Library.Responses;
using MassTransit;
using Microsoft.AspNetCore.Http;
using BackPack.Dependency.Library.Messages;

namespace BackPack.Tenant.Library.Services.Services
{
    public class DashboardService(
        IRequestClient<GetSuperAdminDashboardEvent> requestClient,
        IGetTenantByTenantIDRepository getTenantByTenantIDRepository,
        IGetAnActiveTenantRepository getAnActiveTenantRepository
        ) : IDashboardService
    {
        #region SuperAdminDashboardAsync
        public async Task<GetSuperAdminDashboardAcceptedEvent> SuperAdminDashboardAsync()
        {
            var dbConnection = "";
            var activeTenantID = await getAnActiveTenantRepository.GetAnActiveTenantAsync();
            if (activeTenantID.ToString() != "")
            {
                GetTenantByTenantNameResponse dbResponse = await getTenantByTenantIDRepository.GetTenantDBConnection(tenantID: activeTenantID);
                dbConnection = dbResponse.Data.DBConnection;
            }

            if (dbConnection != "")
            {
                try
                {
                    Response<GetSuperAdminDashboardAcceptedEvent> consumerResponse = await requestClient!.GetResponse<GetSuperAdminDashboardAcceptedEvent>(new GetSuperAdminDashboardEvent()
                    {
                        DBConnection = dbConnection
                    }, timeout: RequestTimeout.After(s: 60));

                    return new GetSuperAdminDashboardAcceptedEvent()
                    {
                        Success = consumerResponse.Message.Success,
                        StatusCode = consumerResponse.Message.StatusCode,
                        Data = consumerResponse.Message.Data,
                        StatusMessage = consumerResponse.Message.StatusMessage,
                        ExceptionMessage = consumerResponse.Message.ExceptionMessage,
                        ExceptionType = consumerResponse.Message.ExceptionType,
                    };
                }
                catch (Exception)
                {
                    return new GetSuperAdminDashboardAcceptedEvent()
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusMessage = CommonMessage.BadRequestMessage
                    };
                }
            }            

            return new GetSuperAdminDashboardAcceptedEvent()
            {
                Success = false,
                StatusCode = StatusCodes.Status400BadRequest,
                StatusMessage = CommonMessage.BadRequestMessage
            };
        }
        #endregion
    }
}
