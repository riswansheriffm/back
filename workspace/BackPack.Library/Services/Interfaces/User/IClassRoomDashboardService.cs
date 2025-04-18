using BackPack.Library.Responses.Dashboard;

namespace BackPack.Library.Services.Interfaces.User
{
    public interface IClassRoomDashboardService
    {
        Task<ClassRoomDashboardResponse> ClassRoomDashboardResponseAsync(int LoginID);

        Task<ClassRoomUpcomingAssignmentsResponse> ClassRoomUpcomingAssignmentsResponseAsync(int AuthorID, string AssignmentDate);
    }
}
