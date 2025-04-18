using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Master.Course
{
    public class AllCourseCapsulesResponse : ReadBaseResponse
    {
        public AllCourseCapsulesResponseData Data { get; set; } = new();
    }
    public class AllCourseCapsulesResponseData
    {
        public List<GetAllCourseCapsulesResult> GetAllCourseCapsulesResult { get; set; } = [];
    }
    public class GetAllCourseCapsulesResult
    {
        public int CourseCapsuleID { get; set; }
        public string? CourseCapsuleName { get; set; }
        public string? CourseCapsuleDesc { get; set; }
        public int ActiveFlag { get; set; }
        public int Published { get; set; }
        public int FolderCount { get; set; }
        public int PodCount { get; set; }
        public string? AccessType { get; set; }
        public int PublishedType { get; set; }
    }
}
