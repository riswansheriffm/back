using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Master.Course
{
    public class AllCourseCapsulesForASubjectResponse : ReadBaseResponse
    {
        public AllCourseCapsulesForASubjectResponseData Data { get; set; } = new();
    }
    public class AllCourseCapsulesForASubjectResponseData
    {
        public List<GetAllCourseCapsulesForASubjectResult> GetAllCourseCapsulesForASubjectResult { get; set; } = [];
    }
    public class GetAllCourseCapsulesForASubjectResult
    {
        public int CourseCapsuleID { get; set; }
        public string? CourseCapsuleName { get; set; }
        public string? CourseCapsuleDesc { get; set; }
        public string? AppType { get; set; }
    }
}
