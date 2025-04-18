
using BackPack.MessageContract.Library;

namespace BackPack.Library.Services.Interfaces.MassTransitMessage
{
    public interface IMassTransitMessageService
    {
        Task<GetTenantResponseEvent> GetTenantEventResponse(string tenantName);
    }
}
