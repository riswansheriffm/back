using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.User
{
    public class DistrictAdminDashboardResponse : ReadBaseResponse
    {
        public DistrictAdminDashboardResponseData? Data { get; set; }
    }
    public class DistrictAdminDashboardResponseData
    {
        public GetDistrictAdminDashBoardResult? GetDistrictAdminDashBoardResults { get; set; }
    }
    public class GetDistrictAdminDashBoardResult
    {
        public int TotalAssignments { get; set; }
        public int TotalSchools { get; set; }
        public int TotalStudents { get; set; }
        public int TotalCourses { get; set; }
        public int TotalContents { get; set; }
        public int TotalTeachers { get; set; }
        public List<ActivityList>? ActivityList { get; set; }
        public List<SchoolSummary>? SchoolList { get; set; }
    }
    public class SchoolSummary
    {
        public string? SchoolName { get; set; }
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
    }
}
