using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Master.Course
{
    public class LPStudentLicensesByCourseCapsuleResponse : ReadBaseResponse
    {
        public LPStudentLicensesByCourseCapsuleResponseData Data { get; set; } = new();
    }
    public class LPStudentLicensesByCourseCapsuleResponseData
    {
        public List<LPStudentLicensesByCourse> GetAllLPStudentLicensesByCourseCapsuleResult { get; set; } = [];
    }
    public class LPStudentLicensesByCourse
    {
        public int CourseID { get; set; }
        public int ID { get; set; }
        public string? LoginName { get; set; } = string.Empty;
        public string? FullName { get; set; } = string.Empty;
        public string? FName { get; set; } = string.Empty;
        public string? LName { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string? SubscriptionDate { get; set; } = string.Empty;
        public string? ExpiryDate { get; set; } = string.Empty;
        public int Status { get; set; }
        public int LicenseStatus { get; set; }
    }
}
