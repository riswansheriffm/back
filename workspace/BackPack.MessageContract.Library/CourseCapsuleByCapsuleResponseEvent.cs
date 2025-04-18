
using BackPack.Dependency.Library.Responses;

namespace BackPack.MessageContract.Library
{
    public class CourseCapsuleByCapsuleResponseEvent : ReadBaseResponse
    {
        public CourseCapsuleByCapsuleResponseEventResult Data { get; set; } = new();
    }

    public class CourseCapsuleByCapsuleResponseEventResult
    {
        public CourseCapsuleByCapsuleResponseEventData GetCourseCapsuleByCapsuleResult { get; set; } = new();
    }

    public class CourseCapsuleByCapsuleResponseEventData
    {
        public int CourseCapsuleID { get; set; }
        public int SubjectID { get; set; }
        public int DomainID { get; set; }
        public int PublishedType { get; set; }
        public string CourseCapsuleName { get; set; } = string.Empty;
        public string CourseCapsuleDesc { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public string AppType { get; set; } = string.Empty;
    }
}
