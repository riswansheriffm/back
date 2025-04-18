using BackPack.Library.Responses.Activity;
using BackPack.Library.Responses.Dashboard;
using BackPack.Library.Responses.User;
using BackPack.MessageContract.Library;

namespace BackPack.Library.Services.Interfaces.User
{
    public interface IDashboardService
    {
        Task<StudentDashboardResponse> StudentDashboardResponseAsync(int LoginID);

        Task<UpcomingAssignmentsResponse> UpcomingAssignmentsResponseAsync(int StudentID, string AssignmentDate);

        Task<GetSuperAdminDashboardAcceptedEvent> SuperAdminDashboardAsync();

        Task<DistrictAdminDashboardResponse> DistrictAdminDashboardAsync(int DomainID, int LoginID);

        Task<CurriculumAdminDashBoardResponse> CurriculumAdminDashboardAsync(int DomainID, int LoginID);

        Task<SchoolAdminDashboardResponse> SchoolAdminDashboardAsync(int SchoolID, int LoginID);
    }
}
