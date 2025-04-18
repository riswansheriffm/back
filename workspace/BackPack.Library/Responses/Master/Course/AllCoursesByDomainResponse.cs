using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Master.Course
{
    public class AllCoursesByDomainResponse : ReadBaseResponse
    {
        public AllCoursesByDomainResponseData? Data { get; set; }
    }
    public class AllCoursesByDomainResponseData
    {
        public List<GetAllCoursesByDomainResult>? GetAllCoursesByDomainResult { get; set; }
    }
    public class GetAllCoursesByDomainResult
    {
        public int CourseID { get; set; }
        public string? CourseName { get; set; }
    }
}
