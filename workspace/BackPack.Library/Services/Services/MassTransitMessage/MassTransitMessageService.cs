
using BackPack.Dependency.Library.Helpers;
using BackPack.Library.Services.Interfaces.MassTransitMessage;
using BackPack.MessageContract.Library;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace BackPack.Library.Services.Services.MassTransitMessage
{
    public class MassTransitMessageService(IRequestClient<GetTenantEvent> requestClient, IConfiguration configuration) : IMassTransitMessageService
    {
        public async Task<GetTenantResponseEvent> GetTenantEventResponse(string tenantName)
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
            try
            {
                Response<GetTenantResponseEvent> consumerResponse = await requestClient.GetResponse<GetTenantResponseEvent>(new GetTenantEvent()
                {
                    TenantName = tenantName
                }, timeout: RequestTimeout.After(s: 60));

                return new GetTenantResponseEvent()
                {
                    TenantID = consumerResponse.Message.TenantID,
                    TenantName = consumerResponse.Message.TenantName,
                    DBConnection = consumerResponse.Message.DBConnection,
                };
            }
            catch (Exception)
            {
                return new GetTenantResponseEvent()
                {

                };
            }

            
        }
    }
}
