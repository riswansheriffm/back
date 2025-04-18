
using BackPack.Dependency.Library.Responses;

namespace BackPack.MessageContract.Library
{
    public class LPCourseLicensesResponseEvent : ReadBaseResponse
    {
        public LPCourseLicensesResponseEventData Data { get; set; } = new();
    }

    public class LPCourseLicensesResponseEventData
    {
        public List<LPCourseLicensesResponseEventResult> GetAllLPCourseLicensesResult { get; set; } = [];
    }
    public class LPCourseLicensesResponseEventResult
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
