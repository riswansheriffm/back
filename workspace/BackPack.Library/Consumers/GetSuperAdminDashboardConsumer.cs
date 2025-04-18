using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Services.Interfaces.User;
using BackPack.MessageContract.Library;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace BackPack.Library.Consumers
{
    public class GetSuperAdminDashboardConsumer(
        IDashboardService dashboardService,
        IConfiguration configuration
        ) : IConsumer<GetSuperAdminDashboardEvent>
    {
        public async Task Consume(ConsumeContext<GetSuperAdminDashboardEvent> context)
        {
            Aes256Helper aes256Helper = new(configuration);
            GlobalApplicationProperty.DBConnection = aes256Helper.Aes256Decryption(context.Message.DBConnection!);

            GetSuperAdminDashboardAcceptedEvent response = await dashboardService.SuperAdminDashboardAsync();

            if (context.IsResponseAccepted<GetSuperAdminDashboardAcceptedEvent>())
            {
                await context.RespondAsync(new GetSuperAdminDashboardAcceptedEvent
                {
                    Success = response.Success,
                    StatusCode = response.StatusCode,
                    Data = response.Data,
                    StatusMessage = response.StatusMessage,
                    ExceptionMessage = response.ExceptionMessage,
                    ExceptionType = response.ExceptionType,
                });
            }
        }
    }
}
