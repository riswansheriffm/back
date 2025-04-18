using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Report
{
    public class LessonPodSummaryAnalyticsReportResponse : ReadBaseResponse
    {
        public LessonPodSummaryAnalyticsReportData Data { get; set; } = new();
    }

    public class LessonPodSummaryAnalyticsReportData : AnalyticsReportSummaryData
    {
        public string LessonName { get; set; } = string.Empty;
        public string LessonDesc { get; set; } = string.Empty;
        public List<string> LessonPodActivityName { get; set; } = new();
        public AnalyticsReportSummaryAverageData StudentLessonPodAverage { get; set; } = new();
        public AnalyticsReportSummaryAverageData PracticeStudentLessonPodAverage { get; set; } = new();
        public List<LessonPodSummaryAnalyticsReportActivityData> Activity { get; set; } = new();
    }

    public class LessonPodSummaryAnalyticsReportActivityData
    {
        public int ContentID { get; set; }
        public int ControlCount { get; set; }
        public float StudentScore { get; set; }
        public float ClassScore { get; set; }
        public float StudentTimelyCompletion { get; set; }
        public float ClassTimelyCompletion { get; set; }
        public float PracticeStudentScore { get; set; }
        public float PracticeClassScore { get; set; }
        public float PracticeStudentTimelyCompletion { get; set; }
        public float PracticeClassTimelyCompletion { get; set; }
        public string ContentName { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty;
        public string AppName { get; set; } = string.Empty;
        public string StudentTimeSpent { get; set; } = string.Empty;
        public string ClassTimeSpent { get; set; } = string.Empty;
        public string ActivityStatus { get; set; } = string.Empty;
        public string PracticeStudentTimeSpent { get; set; } = string.Empty;
        public string PracticeClassTimeSpent { get; set; } = string.Empty;
    }

    public class LessonPodSummaryAnalyticsReportQueryResponseData
    {
        public int ContentID { get; set; }
        public int TestStudentTimelyCompletion { get; set; }
        public int TestStudentTimeTaken { get; set; }
        public int TestStudentScore { get; set; }
        public int TestClassTimeTaken { get; set; }
        public int TestClassAverageScore { get; set; }
        public int TestClassAverageTimelyCompletion { get; set; }
        public int PrStudentMasteryIndex { get; set; }
        public int PrStudentTimelyCompletion { get; set; }
        public int PrClassAvgTimelyCompletion { get; set; }
        public int PrClassAvgMasteryIndex { get; set; }
        public int StudentCumulativeTimeTaken { get; set; }
        public int ClassAverageCumulativeTimeTaken { get; set; }
        public int ControlCount { get; set; }
        public string ContentName { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty;
        public string AppName { get; set; } = string.Empty;
        public string TestStudentTimeSpent { get; set; } = string.Empty;
        public string TestClassTimeSpent { get; set; } = string.Empty;
        public string PrStudentTimeSpent { get; set; } = string.Empty;
        public string PrClassTimeSpent { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
