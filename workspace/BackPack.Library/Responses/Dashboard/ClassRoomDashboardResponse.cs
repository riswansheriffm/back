using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Dashboard
{
    public class ClassRoomDashboardResponse : ReadBaseResponse
    {
        public ClassRoomDashboardResult Data { get; set; } = new();
    }

    public class ClassRoomDashboardResult
    {
        public ClassRoomDashboardData GetTeacherDashboardResult { get; set; } = new();
    }

    public class ClassRoomDashboardData
    {
        public int CompletedQuestions { get; set; }
        public int CompletedActivities { get; set; }
        public int TotalTime { get; set; }
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalClasses { get; set; }
        public string Score { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string TotalTimeTaken { get; set; } = string.Empty;
        public string TotalTimeTakenMin { get; set; } = string.Empty;
        public string TotalTimeTakenSec { get; set; } = string.Empty;
        public List<ClassRoomDashboardCourseData> Course { get; set; } = new();
    }

    public class ClassRoomDashboardCourseData
    {
        public int CourseID { get; set; }
        public int NoOfStudents { get; set; }
        public int SubjectID { get; set; }
        public int CompletedQuestions { get; set; }
        public int CompletedActivities { get; set; }
        public int TotalTime { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string CourseDesc { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public string Score { get; set; } = string.Empty;
        public string TotalTimeTaken { get; set; } = string.Empty;
        public string TotalTimeTakenMin { get; set; } = string.Empty;
        public string TotalTimeTakenSec { get; set; } = string.Empty;
    }

    public class ClassRoomDashboardTeacherData
    {
        public string FullName { get; set; } = string.Empty;
    }
}
