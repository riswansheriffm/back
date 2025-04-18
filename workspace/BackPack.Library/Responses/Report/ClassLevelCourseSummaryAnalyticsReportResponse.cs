using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Report
{
    public class ClassLevelCourseSummaryAnalyticsReportResponse : ReadBaseResponse
    {
        public ClassLevelCourseSummaryAnalyticsReportData Data { get; set; } = new();
    }

    public class ClassLevelCourseSummaryAnalyticsReportData
    {
        public string AverageTimeSpent { get; set; } = string.Empty;
        public string AverageTimeTaken { get; set; } = string.Empty;
        public string AverageScore { get; set; } = string.Empty;
        public string AverageTimelyCompletion { get; set; } = string.Empty;
        public string PracticeAverageTimeSpent { get; set; } = string.Empty;
        public string PracticeAverageTimeTaken { get; set; } = string.Empty;
        public string PracticeAverageScore { get; set; } = string.Empty;
        public string PracticeAverageTimelyCompletion { get; set; } = string.Empty;
        public List<int> StudentAverageTimeTaken { get; set; } = new();
        public List<int> PracticeStudentAverageTimeTaken { get; set; } = new();
        public List<string> StudentsLoginName { get; set; } = new();
        public List<string> StudentsFullName { get; set; } = new();
        public List<string> StudentAverageSpentTime { get; set; } = new();
        public List<string> StudentAverageScore { get; set; } = new();
        public List<string> StudentAverageTimelyCompletion { get; set; } = new();
        public List<string> StudentSpentTime { get; set; } = new();
        public List<string> StudentTimeTaken { get; set; } = new();
        public List<string> StudentScore { get; set; } = new();
        public List<string> StudentTimelyCompletion { get; set; } = new();
        public List<string> PracticeStudentAverageSpentTime { get; set; } = new();
        public List<string> PracticeStudentAverageScore { get; set; } = new();
        public List<string> PracticeStudentAverageTimelyCompletion { get; set; } = new();
        public List<string> PracticeStudentSpentTime { get; set; } = new();
        public List<string> PracticeStudentTimeTaken { get; set; } = new();
        public List<string> PracticeStudentScore { get; set; } = new();
        public List<string> PracticeStudentTimelyCompletion { get; set; } = new();
        public List<ClassLevelCourseSummaryAnalyticsReportStudent> StudentSummary { get; set; } = new();
    }

    public class ClassLevelCourseSummaryAnalyticsReportStudent
    {
        public int StudentID { get; set; }
        public string LoginName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string StudentScore { get; set; } = string.Empty;
        public string StudentTimeSpent { get; set; } = string.Empty;
        public string StudentTimeTaken { get; set; } = string.Empty;
        public string StudentTimelyCompletion { get; set; } = string.Empty;
        public string PracticeStudentScore { get; set; } = string.Empty;
        public string PracticeStudentTimeSpent { get; set; } = string.Empty;
        public string PracticeStudentTimeTaken { get; set; } = string.Empty;
        public string PracticeStudentTimelyCompletion { get; set; } = string.Empty;
    }
    public class ClassLevelCourseSummaryAnalyticsReportQueryData
    {
        public int StudentID { get; set; }
        public string LoginName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string TestStudentAverageScore { get; set; } = string.Empty;
        public string TestStudentAverageTimeSpent { get; set; } = string.Empty;
        public string TestStudentTimeTaken { get; set; } = string.Empty;
        public string TestStudentAverageTimelyCompletion { get; set; } = string.Empty;
        public string PracticeStudentAverageScore { get; set; } = string.Empty;
        public string PracticeStudentAverageTimeSpent { get; set; } = string.Empty;
        public string PracticeStudentTimeTaken { get; set; } = string.Empty;
        public string PracticeStudentAverageTimelyCompletion { get; set; } = string.Empty;
    }
}
