using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Dashboard
{
    public class StudentDashboardResponse : ReadBaseResponse
    {
        public StudentDashboardDataResult Data { get; set; } = new();
    }

    public class StudentDashboardDataResult
    {
        public StudentDashboardDataResponse GetBPStudentDashboardResult { get; set; } = new();
    }

    public class StudentDashboardDataResponse
    {
        public int StudentID { get; set; }
        public int CompletionPercentage { get; set; }
        public string? UserName { get; set; }
        public List<StudentDashboardCourseResponse>? Course { get; set; } 
    }

    public class StudentDashboardCourseResponse
    {
        public int CourseID { get; set; }
        public int CourseType { get; set; }
        public int SubjectID { get; set; }
        public int NoOfStudents { get; set; }
        public string? CourseName { get; set; }
        public string? CourseDesc { get; set; }
        public string? TeacherName { get; set; }
        public string? AverageScore { get; set; }
        public string? AverageTimeSpent { get; set; }
        public string? AverageTimeTaken { get; set; }
        public string? AverageTimelyCompletion { get; set; }
        public string? ActivityCount { get; set; }
        public string? ImageURL { get; set; }
    }

    public class StudentDashboardDataReadResponse
    {
        public int ID { get; set; }
        public int CompletionPercentage { get; set; }
        public string? FullName { get; set; }
    }
}
