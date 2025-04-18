using BackPack.Library.Repositories.Interfaces.Activity;
using BackPack.Library.Repositories.Interfaces.Dashboard;
using BackPack.Library.Responses.Activity;
using BackPack.Library.Responses.Dashboard;
using BackPack.Library.Responses.User;
using BackPack.Library.Services.Interfaces.User;
using BackPack.MessageContract.Library;

namespace BackPack.Library.Services.Services.User
{
    public class DashboardService(
        IStudentDashboardRepository studentDashboardRepository,
        IUpcomingAssignmentRepository upcomingAssignmentRepository,
        IAdminDashboardRepository adminDashboardRepository
        ) : IDashboardService
    {
        #region StudentDashboardResponseAsync
        public async Task<StudentDashboardResponse> StudentDashboardResponseAsync(int LoginID)
        {
            StudentDashboardResponse response = await studentDashboardRepository.StudentDashboardResponseAsync(LoginID);

            return response;
        }
        #endregion

        #region UpcomingAssignmentsResponseAsync
        public async Task<UpcomingAssignmentsResponse> UpcomingAssignmentsResponseAsync(int StudentID, string AssignmentDate)
        {
            UpcomingAssignmentsResponse response = await upcomingAssignmentRepository.UpcomingAssignmentsResponseAsync(StudentID, AssignmentDate);

            return response;
        }
        #endregion

        #region SuperAdminDashboardAsync
        public async Task<GetSuperAdminDashboardAcceptedEvent> SuperAdminDashboardAsync()
        {
            GetSuperAdminDashboardAcceptedEvent response = await adminDashboardRepository.SuperAdminDashboardAsync();

            return response;
        }
        #endregion

        #region DistrictAdminDashboardAsync
        public async Task<DistrictAdminDashboardResponse> DistrictAdminDashboardAsync(int DomainID, int LoginID)
        {
            DistrictAdminDashboardResponse response = await adminDashboardRepository.DistrictAdminDashboardAsync(DomainID, LoginID);

            return response;
        }
        #endregion

        #region CurriculumAdminDashboardAsync
        public async Task<CurriculumAdminDashBoardResponse> CurriculumAdminDashboardAsync(int DomainID, int LoginID)
        {
            CurriculumAdminDashBoardResponse response = await adminDashboardRepository.CurriculumAdminDashboardAsync(DomainID, LoginID);

            return response;
        }
        #endregion

        #region SchoolAdminDashboardAsync
        public async Task<SchoolAdminDashboardResponse> SchoolAdminDashboardAsync(int SchoolID, int LoginID)
        {
            SchoolAdminDashboardResponse response = await adminDashboardRepository.SchoolAdminDashboardAsync(SchoolID, LoginID);

            return response;
        }
        #endregion
    }
}
