using BackPack.Library.Responses.Dashboard;

namespace BackPack.Library.Repositories.Interfaces.Dashboard
{
    public interface IStudentDashboardRepository
    {
        Task<StudentDashboardResponse> StudentDashboardResponseAsync(int LoginID);
    }
}
