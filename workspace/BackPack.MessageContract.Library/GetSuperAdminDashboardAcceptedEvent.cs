using BackPack.Dependency.Library.Responses;

namespace BackPack.MessageContract.Library
{
    public class GetSuperAdminDashboardAcceptedEvent : ReadBaseResponse
    {
        public SuperAdminDashboardResponseData? Data { get; set; }
    }

    public class SuperAdminDashboardResponseData
    {
        public GetSuperAdminDashboardResult? GetSuperAdminDashboardResults { get; set; }
    }

    public class GetSuperAdminDashboardResult
    {
        public int TotalDistricts { get; set; }
        public int TotalSchools { get; set; }
        public int TotalStudents { get; set; }
        public int TotalCourses { get; set; }
        public int TotalContents { get; set; }
        public int TotalTeachers { get; set; }
        public List<TenantActivityList>? ActivityList { get; set; }
        public List<TenantDistrictSummary>? DistrictSummary { get; set; }
    }

    public class TenantActivityList
    {
        public string? Desc { get; set; }
        public int ID { get; set; }
    }

    public class TenantDistrictSummary
    {
        public string? DistrictName { get; set; }
        public int TotalSchools { get; set; }
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
    }
}
