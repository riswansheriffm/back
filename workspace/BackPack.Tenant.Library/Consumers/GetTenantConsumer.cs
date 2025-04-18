using BackPack.MessageContract.Library;
using BackPack.Tenant.Library.Repositories.Interfaces;
using BackPack.Tenant.Library.Responses;
using MassTransit;

namespace BackPack.Tenant.Library.Consumers
{
    public class GetTenantConsumer(IGetTenantByTenantNameRepository getTenantByTenantNameRepository) : IConsumer<GetTenantEvent>
    {
        public async Task Consume(ConsumeContext<GetTenantEvent> context)
        {
            try
            {
                GetTenantByTenantNameResponse response = await getTenantByTenantNameRepository.GetTenantDBConnection(tenantName: context.Message.TenantName);
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
            catch (Exception)
            {
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
