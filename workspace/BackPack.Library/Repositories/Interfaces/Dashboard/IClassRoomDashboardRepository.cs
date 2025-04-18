using BackPack.Library.Responses.Dashboard;

namespace BackPack.Library.Repositories.Interfaces.Dashboard
{
    public interface IClassRoomDashboardRepository
    {
        Task<ClassRoomDashboardResponse> ClassRoomDashboardResponseAsync(int LoginID);        
    }
}
