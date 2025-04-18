using BackPack.Library.Responses.User;
using BackPack.MessageContract.Library;

namespace BackPack.Library.Repositories.Interfaces.Dashboard
{
    public interface IAdminDashboardRepository
    {
        Task<GetSuperAdminDashboardAcceptedEvent> SuperAdminDashboardAsync();

        Task<DistrictAdminDashboardResponse> DistrictAdminDashboardAsync(int DomainID, int LoginID);

        Task<CurriculumAdminDashBoardResponse> CurriculumAdminDashboardAsync(int DomainID, int LoginID);

        Task<SchoolAdminDashboardResponse> SchoolAdminDashboardAsync(int SchoolID, int LoginID);
    }
}
