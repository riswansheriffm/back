using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Report
{
    public class CourseSummaryAnalyticsReportResponse : ReadBaseResponse
    {
        public CourseSummaryAnalyticsReportData Data { get; set; } = new();
    }

    public class CourseSummaryAnalyticsReportData : AnalyticsReportSummaryData
    {
        public List<string> LessonPodName { get; set; } = new();
        public AnalyticsReportSummaryAverageData StudentCourseAverage { get; set; } = new();
        public AnalyticsReportSummaryAverageData PracticeStudentCourseAverage { get; set; } = new();
        public List<CourseSummaryAnalyticsReportLessonFolderData> LessonFolders { get; set; } = new();
    }

    public class AnalyticsReportSummaryData
    {
        public List<int> StudentTimeTaken { get; set; } = new();
        public List<int> ClassTimeTaken { get; set; } = new();
        public List<int> PracticeStudentTimeTaken { get; set; } = new();
        public List<int> PracticeClassTimeTaken { get; set; } = new();
        public List<string> StudentSpentTime { get; set; } = new();
        public List<string> ClassSpentTime { get; set; } = new();
        public List<string> StudentAverageScore { get; set; } = new();
        public List<string> ClassAverageScore { get; set; } = new();
        public List<string> StudentScore { get; set; } = new();
        public List<string> ClassScore { get; set; } = new();
        public List<string> StudentAverageTimelyCompletion { get; set; } = new();
        public List<string> ClassAverageTimelyCompletion { get; set; } = new();
        public List<string> StudentTimelyCompletion { get; set; } = new();
        public List<string> ClassTimelyCompletion { get; set; } = new();
        public List<string> PracticeStudentSpentTime { get; set; } = new();
        public List<string> PracticeClassSpentTime { get; set; } = new();
        public List<string> PracticeStudentTimelyCompletion { get; set; } = new();
        public List<string> PracticeClassTimelyCompletion { get; set; } = new();
        public List<string> PracticeStudentAverageTimelyCompletion { get; set; } = new();
        public List<string> PracticeClassAverageTimelyCompletion { get; set; } = new();
        public List<string> PracticeStudentAverageScore { get; set; } = new();
        public List<string> PracticeClassAverageScore { get; set; } = new();
        public List<string> PracticeStudentScore { get; set; } = new();
        public List<string> PracticeClassScore { get; set; } = new();
    }

    public class CourseSummaryAnalyticsReportQueryResponseData
    {
        public int LessonID { get; set; }
        public int LessonUnitDistID { get; set; }
        public int DisplayOrder { get; set; }
        public int ReworkCount { get; set; }
        public int TestStudentTimeTaken { get; set; }
        public int TestClassTimeTaken { get; set; }
        public int TestCount { get; set; }
        public int PracticeCount { get; set; }
        public int StudentAverageCumulativeTimeTaken { get; set; }
        public int ClassAverageCumulativeTimeTaken { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public string LessonDesc { get; set; } = string.Empty;
        public string LessonPodName { get; set; } = string.Empty;
        public string LessonPodDesc { get; set; } = string.Empty;
        public string LessonPodStatus { get; set; } = string.Empty;
        public string TestStudentTimeSpent { get; set; } = string.Empty;
        public string TestStudentAverageScore { get; set; } = string.Empty;
        public string TestStudentAverageTimelyCompletion { get; set; } = string.Empty;
        public string LessonPodCompletion { get; set; } = string.Empty;
        public string TestClassTimeSpent { get; set; } = string.Empty;
        public string TestClassAverageScore { get; set; } = string.Empty;
        public string TestClassAverageTimelyCompletion { get; set; } = string.Empty;
        public string PrStudentTimeSpent { get; set; } = string.Empty;
        public string PrStudentAvgMasteryIndex { get; set; } = string.Empty;
        public string PrStudentAvgTimelyCompletion { get; set; } = string.Empty;
        public string PrStudentTotalTries { get; set; } = string.Empty;
        public string PrClassTimeSpent { get; set; } = string.Empty;
        public string PrClassAvgMasteryIndex { get; set; } = string.Empty;
        public string PrClassAvgTimelyCompletion { get; set; } = string.Empty;
        public string PrClassTotalTries { get; set; } = string.Empty;
    }

    public class AnalyticsReportSummaryAverageData
    {
        public string AverageScore { get; set; } = string.Empty;
        public string AverageTimeSpent { get; set; } = string.Empty;
        public string AverageTimelyCompletion { get; set; } = string.Empty;
        public string AverageTimeTaken { get; set; } = string.Empty;
    }

    public class CourseSummaryAnalyticsReportLessonFolderData
    {
        public int LessonID { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public string LessonDesc { get; set; } = string.Empty;
        public List<CourseSummaryAnalyticsReportLessonFolderLessonPodData> LessonPods { get; set; } = new();
    }

    public class CourseSummaryAnalyticsReportLessonFolderLessonPodData
    {
        public int LessonUnitDistID { get; set; }
        public int ReworkCount { get; set; }
        public int TestCount { get; set; }
        public int PracticeCount { get; set; }
        public string LessonPodStatus { get; set; } = string.Empty;
        public string LessonPodName { get; set; } = string.Empty;
        public string LessonPodDesc { get; set; } = string.Empty;
        public string StudentTimeSpent { get; set; } = string.Empty;
        public string StudentScore { get; set; } = string.Empty;
        public string StudentTimelyCompletion { get; set; } = string.Empty;
        public string ClassTimeSpent { get; set; } = string.Empty;
        public string ClassScore { get; set; } = string.Empty;
        public string ClassTimelyCompletion { get; set; } = string.Empty;
        public string LessonPodCompletion { get; set; } = string.Empty;
        public string PracticeStudentTimeSpent { get; set; } = string.Empty;
        public string PracticeStudentMasteryIndex { get; set; } = string.Empty;
        public string PracticeTimelyCompletion { get; set; } = string.Empty;
        public string PracticeStudentTotalTries { get; set; } = string.Empty;
        public string PracticeClassTimeSpent { get; set; } = string.Empty;
        public string PracticeClassMasteryIndex { get; set; } = string.Empty;
        public string PracticeClassCompletion { get; set; } = string.Empty;
        public string PracticeClassTotalTries { get; set; } = string.Empty;
    }
}
