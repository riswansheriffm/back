using BackPack.MessageContract.Library;

namespace BackPack.Tenant.Library.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<GetSuperAdminDashboardAcceptedEvent> SuperAdminDashboardAsync();
    }
}
