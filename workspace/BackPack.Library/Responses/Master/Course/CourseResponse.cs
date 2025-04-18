using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Master.Course
{
    public class CourseResponse : ReadBaseResponse
    {
        public CourseResponseData? Data { get; set; }
    }
    public class CourseResponseData
    {
        public GetCourseResult? GetCourseResult { get; set; }
    }
    public class GetCourseResult
    {
        public int CourseID { get; set; }
        public string? CourseName { get; set; } = string.Empty;
        public string? CourseDesc { get; set; } = string.Empty;
        public int CourseType { get; set; }
        public string? ImageURL { get; set; } = string.Empty;
        public int SchoolID { get; set; }
        public int SubjectID { get; set; }
        public List<int>? StudentsList { get; set; }
        public List<int>? TeachersList { get; set; }
    }
    public class Students
    {
        public int StudentID { get; set; }
    }
    public class Teachers
    {
        public int TeacherID { get; set; }
    }
}
