using BackPack.MessageContract.Library;

namespace BackPack.Library.Services.Interfaces.MassTransitMessage
{
    public interface ITenantByTenantIDService
    {
        Task<GetTenantResponseEvent> GetTenantByIDEventResponse(Guid tenantID);
    }
}
