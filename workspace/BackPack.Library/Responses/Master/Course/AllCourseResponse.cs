using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Master.Course
{
    public class AllCourseResponse : ReadBaseResponse
    {
        public AllCourseResponseData? Data { get; set; }
    }
    public class AllCourseResponseData
    {
        public List<GetAllCoursesResult>? GetAllCoursesResult { get; set; }
    }
    public class GetAllCoursesResult
    {
        public string? CourseDesc { get; set; } = string.Empty;
        public string? CourseName { get; set; } = string.Empty;
        public int CourseID { get; set; }
        public int SubjectID { get; set; }
        public string? SubjectName { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
    }
}
