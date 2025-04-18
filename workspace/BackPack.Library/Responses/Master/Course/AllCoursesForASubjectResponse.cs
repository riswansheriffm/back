using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Master.Course
{
    public class AllCoursesForASubjectResponse : ReadBaseResponse
    {
        public AllCoursesForASubjectDataResult Data { get; set; } = new();
    }

    public class AllCoursesForASubjectDataResult
    {
        public List<AllCoursesForASubjectData> GetAllCoursesForASubjectResult { get; set; } = [];
    }

    public class AllCoursesForASubjectData
    {
        public int CourseID { get; set; }
        public int SubjectID { get; set; }
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string CourseDesc { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
    }
}
