using BackPack.Dependency.Library.Helpers;
using BackPack.Library.Services.Interfaces.MassTransitMessage;
using BackPack.MessageContract.Library;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace BackPack.Library.Services.Services.MassTransitMessage
{
    public class TenantByTenantIDService(IRequestClient<GetTenantByTenantIDEvent> requestClient, IConfiguration configuration) : ITenantByTenantIDService 
    {
        public async Task<GetTenantResponseEvent> GetTenantByIDEventResponse(Guid tenantID)
        {
            var transitUrl = configuration.GetSection("CommonSettings").GetSection("TenantUrl").Value;
            bool isActive = await GlobalHelper.ActivateTransitAsync(transitUrl!);
            if (!isActive)
            {
                return new GetTenantResponseEvent()
                {
                    //TenantID = consumerResponse.Message.TenantID,
                    //TenantName = consumerResponse.Message.TenantName,
                    //DBConnection = consumerResponse.Message.DBConnection,
                };
            }

            Response<GetTenantResponseEvent> consumerResponse = await requestClient.GetResponse<GetTenantResponseEvent>(new GetTenantByTenantIDEvent()
            {
                TenantID = tenantID
            }, timeout: RequestTimeout.After(s: 60));

            return new GetTenantResponseEvent()
            {
                TenantID = consumerResponse.Message.TenantID,
                TenantName = consumerResponse.Message.TenantName,
                DBConnection = consumerResponse.Message.DBConnection,
            };
        }
    }
}
