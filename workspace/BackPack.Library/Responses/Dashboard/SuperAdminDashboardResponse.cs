//using BackPack.Dependency.Library.Responses;

//namespace BackPack.Library.Responses.Dashboard
//{
//    public class SuperAdminDashboardResponse : ReadBaseResponse
//    {
//        public SuperAdminDashboardResponseData? Data { get; set; }
//    }
//    public class SuperAdminDashboardResponseData
//    {
//        public GetSuperAdminDashboardResult? GetSuperAdminDashboardResults { get; set; }
//    }
//    public class GetSuperAdminDashboardResult
//    {
//        public int Districts { get; set; }
//        public int Schools { get; set; }
//        public int Students { get; set; }
//        public int Courses { get; set; }
//        public int Contents { get; set; }
//        public int Teachers { get; set; }
//        public List<ActivityList>? ActivityList { get; set; }
//        public List<DistrictSummary>? DistrictSummary { get; set; }
//    }
//    public class ActivityList
//    {
//        public string? ActivityDesc { get; set; }
//        public int ActivityID { get; set; }
//    }
//    public class DistrictSummary
//    {
//        public string? DomainName { get; set; }
//        public int Schools { get; set; }
//        public int Teachers { get; set; }
//        public int Students { get; set; }
//    }
//}
