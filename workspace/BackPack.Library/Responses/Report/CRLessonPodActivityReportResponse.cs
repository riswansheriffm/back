using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Report
{
    public class CRLessonPodActivityReportResponse : ReadBaseResponse
    {
        public CRLessonPodActivityReportData Data { get; set; } = new();
    }

    public class CRLessonPodActivityReportData : CRLessonPodActivityReportSummaryData
    {
        public List<int> TimelyCompletion { get; set; } = new();
        public List<int> SpentTimeSeconds { get; set; } = new();
        public List<float> Score { get; set; } = new();
        public List<float> StudentCount { get; set; } = new();
        public List<string> SpentTime { get; set; } = new();
        public List<string> Students { get; set; } = new();
        public List<string> SpentTimeStudents { get; set; } = new();
        public List<string> ScoreStudents { get; set; } = new();
        public List<string> TimelyCompletionStudents { get; set; } = new();
        public List<string> StudentSummary { get; set; } = new();
        public List<CRLessonPodActivityReportControlData> ActivityControls { get; set; } = new();
    }

    public class CRLessonPodActivityReportSummaryData
    {
        public int ContentID { get; set; }
        public float AverageScore { get; set; }
        public float AverageTimelyCompletion { get; set; }
        public string AverageTimeSpent { get; set; } = string.Empty;
    }

    public class CRLessonPodActivityReportControlData
    {
        public float ControlScorePercent { get; set; }
        public float TotalPoints { get; set; }
        public float StudentScore { get; set; }
        public string PluginName { get; set; } = string.Empty;
        public string ControlTag { get; set; } = string.Empty;
        public string ControlName { get; set; } = string.Empty;
    }

    public class CRLessonPodActivityReportStudentSummaryQueryData
    {
        public int TimelyCompletion { get; set; }
        public int TimeTaken { get; set; }
        public float ActivityScore { get; set; }
        public float AverageTimelyCompletion { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string TimeSpent { get; set; } = string.Empty;
        public string TimelyCompletionBool { get; set; } = string.Empty;
    }

    public class CRLessonPodActivityReportActivitySummaryQueryData
    {
        public int StudentCount { get; set; }
        public string StudentSummary { get; set; } = string.Empty;
    }
}
