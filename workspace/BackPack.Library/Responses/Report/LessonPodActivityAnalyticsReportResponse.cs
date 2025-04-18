using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Report
{
    public class LessonPodActivityAnalyticsReportResponse : ReadBaseResponse
    {
        public LessonPodActivityAnalyticsReportData Data { get; set; } = new();
    }

    public class LessonPodActivityAnalyticsReportData : LessonPodActivityAnalyticsReportQueryData
    {
        public List<LessonPodActivityAnalyticsReportControlData> ActivityControls { get; set; } = new();
    }

    public class LessonPodActivityAnalyticsReportQueryData
    {
        public int ContentID { get; set; }
        public string AppName { get; set; } = string.Empty;
        public string SearchName { get; set; } = string.Empty;
        public string SearchTag { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string LoginName { get; set; } = string.Empty;
        public string FollowTheFlow { get; set; } = string.Empty;
        public string ClassTimeSpent { get; set; } = string.Empty;
        public string ClassTimeTaken { get; set; } = string.Empty;
        public string ClassScore { get; set; } = string.Empty;
        public string ClassTimelyCompletion { get; set; } = string.Empty;
        public string StudentTimeSpent { get; set; } = string.Empty;
        public string StudentTimeTaken { get; set; } = string.Empty;
        public string StudentScore { get; set; } = string.Empty;
        public string StudentTimelyCompletion { get; set; } = string.Empty;
    }

    public class LessonPodActivityAnalyticsReportControlData
    {
        public string PluginName { get; set; } = string.Empty;
        public string ControlTag { get; set; } = string.Empty;
        public string ControlName { get; set; } = string.Empty;
        public string ClassControlScore { get; set; } = string.Empty;
        public string StudentControlScore { get; set; } = string.Empty;
    }
}
