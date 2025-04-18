using BackPack.Dependency.Library.Responses;
using BackPack.Library.Responses.Dashboard;

namespace BackPack.Library.Responses.User
{
    public class SchoolAdminDashboardResponse : ReadBaseResponse
    {
        public SchoolAdminDashboardResponseData Data { get; set; } = new();
    }
    public class SchoolAdminDashboardResponseData
    {
        public GetSchoolAdminDashboardResult GetSchoolAdminDashboardResult { get; set; } = new();
    }
    public class GetSchoolAdminDashboardResult
    {
        public int TotalTeachers { get; set; }
        public int TotalStudents { get; set; }
        public int TotalCourses { get; set; }
        public List<ActivityList>? ActivityList { get; set; }
        public List<CourseList>? CourseList { get; set; }
    }
    public class CourseList
    {
        public string CourseName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
    }
}
