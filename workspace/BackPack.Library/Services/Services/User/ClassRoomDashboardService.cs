using BackPack.Library.Repositories.Interfaces.Dashboard;
using BackPack.Library.Responses.Dashboard;
using BackPack.Library.Services.Interfaces.User;

namespace BackPack.Library.Services.Services.User
{
    public class ClassRoomDashboardService(
        IClassRoomDashboardRepository classRoomDashboardRepository,
        IClassRoomUpcomingAssignmentRepository classRoomUpcomingAssignmentRepository
        ) : IClassRoomDashboardService
    {
        #region ClassRoomDashboardResponseAsync        
        public async Task<ClassRoomDashboardResponse> ClassRoomDashboardResponseAsync(int LoginID)
        {
            ClassRoomDashboardResponse response = await classRoomDashboardRepository.ClassRoomDashboardResponseAsync(LoginID);

            return response;
        }
        #endregion

        #region ClassRoomUpcomingAssignmentsResponseAsync        
        public async Task<ClassRoomUpcomingAssignmentsResponse> ClassRoomUpcomingAssignmentsResponseAsync(int AuthorID, string AssignmentDate)
        {
            ClassRoomUpcomingAssignmentsResponse response = await classRoomUpcomingAssignmentRepository.ClassRoomUpcomingAssignmentsResponseAsync(AuthorID, AssignmentDate);

            return response;
        }
        #endregion
    }
}
