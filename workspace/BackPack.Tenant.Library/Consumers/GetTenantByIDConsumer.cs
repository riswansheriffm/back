using BackPack.MessageContract.Library;
using BackPack.Tenant.Library.Repositories.Interfaces;
using BackPack.Tenant.Library.Responses;
using MassTransit;

namespace BackPack.Tenant.Library.Consumers
{
    public class GetTenantByIDConsumer(IGetTenantByTenantIDRepository getTenantByTenantIDRepository) : IConsumer<GetTenantByTenantIDEvent>
    {
        public async Task Consume(ConsumeContext<GetTenantByTenantIDEvent> context)
        {
            GetTenantByTenantNameResponse response = await getTenantByTenantIDRepository.GetTenantDBConnection(tenantID: context.Message.TenantID);

            if (response.Success)
            {
                if (context.IsResponseAccepted<GetTenantResponseEvent>())
                    await context.RespondAsync(new GetTenantResponseEvent()
                    {
                        TenantID = response.Data.TenantID,
                        TenantName = response.Data.TenantName,
                        DBConnection = response.Data.DBConnection,
                    });
            }
            else
            {
                if (context.IsResponseAccepted<GetTenantResponseEvent>())
                    await context.RespondAsync(new GetTenantResponseEvent()
                    {
                        TenantID = Guid.Empty,
                        TenantName = string.Empty,
                        DBConnection = string.Empty,
                    });
            }
        }
    }
}
