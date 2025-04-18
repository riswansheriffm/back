using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Master.Course
{
    public class LPCourseLicensesResponse : ReadBaseResponse
    {
        public LPCourseLicensesResponseData Data { get; set; } = new();
    }
    public class LPCourseLicensesResponseData
    {
        public List<GetAllLPCourseLicensesResult> GetAllLPCourseLicensesResult { get; set; } = [];
    }
    public class GetAllLPCourseLicensesResult
    {
        public int CourseID { get; set; }
        public string? CourseName { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string? SubscriptionDate { get; set; } = string.Empty;
        public string? ExpiryDate { get; set; } = string.Empty;
        public int Status { get; set; }
        public int LicenseStatus { get; set; }
    }
}
