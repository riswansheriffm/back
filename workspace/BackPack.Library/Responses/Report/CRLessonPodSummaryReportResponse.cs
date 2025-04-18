using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Report
{
    public class CRLessonPodSummaryReportResponse : ReadBaseResponse
    {
        public CRLessonPodSummaryReportData Data { get; set; } = new();
    }

    public class CRLessonPodSummaryReportData : CRLessonPodSummaryReportSummaryData
    {
        public List<int> SpentTimeSeconds { get; set; } = new();
        public List<float> Score { get; set; } = new();
        public List<float> TimelyCompletion { get; set; } = new();
        public List<string> SpentTime { get; set; } = new();
        public List<string> Students { get; set; } = new();
        public List<string> SpentTimeStudents { get; set; } = new();
        public List<string> ScoreStudents { get; set; } = new();
        public List<string> TimelyCompletionStudents { get; set; } = new();
        public List<CRLessonPodSummaryReportActivityData> Activity { get; set; } = new();
    }

    public class CRLessonPodSummaryReportSummaryData
    {
        public int LessonUnitDistID { get; set; }
        public float AverageScore { get; set; }
        public float AverageTimelyCompletion { get; set; }
        public string AverageTimeSpent { get; set; } = string.Empty;
    }

    public class CRLessonPodSummaryReportActivityData
    {
        public int ContentID { get; set; }
        public int ControlCount { get; set; }
        public float AverageScore { get; set; }
        public float AverageTimelyCompletion { get; set; }
        public string ContentName { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty;
        public string AppName { get; set; } = string.Empty;
        public string AverageTimeSpent { get; set; } = string.Empty;
    }

    public class CRLessonPodSummaryReportStudentSummaryQueryData
    {
        public int AverageTimeTaken { get; set; }
        public float AverageScore { get; set; }
        public float AverageTimelyCompletion { get; set; }
        public string AverageTimeSpent { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}
