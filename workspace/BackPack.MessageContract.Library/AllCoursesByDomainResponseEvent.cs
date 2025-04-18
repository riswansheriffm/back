
using BackPack.Dependency.Library.Responses;

namespace BackPack.MessageContract.Library
{
    public class AllCoursesByDomainResponseEvent : ReadBaseResponse
    {
        public AllCoursesByDomainResponseEventData Data { get; set; } = new();
    }
    public class AllCoursesByDomainResponseEventData
    {
        public List<AllCoursesByDomainResponseEventResult> GetAllCoursesByDomainResult { get; set; } = [];
    }
    public class AllCoursesByDomainResponseEventResult
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; } = string.Empty;
    }
}
