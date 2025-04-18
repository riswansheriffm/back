using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Report
{
    public class CRDistributedLessonUnitsByLessonReportResponse : ReadBaseResponse
    {
        public CRDistributedLessonUnitsByLessonReportResult Data { get; set; } = new();
    }

    public class CRDistributedLessonUnitsByLessonReportResult
    {
        public CRDistributedLessonUnitsByLessonReportData GetCRDistributedLessonUnitsByLessonReportResult { get; set; } = new();
    }

    public class CRDistributedLessonUnitsByLessonReportData
    {
        public int CompletedQuestions { get; set; }
        public int CompletedActivities { get; set; }
        public int TotalTime { get; set; }
        public int NoOfLessonPods { get; set; }
        public int NoOfStudents { get; set; }
        public int NoOfTeachers { get; set; }
        public string Score { get; set; } = string.Empty;
        public string TotalTimeTaken { get; set; } = string.Empty;
        public string TotalTimeTakenMin { get; set; } = string.Empty;
        public string TotalTimeTakenSec { get; set; } = string.Empty;
        public List<CRDistributedLessonUnitsByLessonReportLessonData> PastAssignments { get; set; } = new();
        public List<CRDistributedLessonUnitsByLessonReportLessonData> UpcomingAssignments { get; set; } = new();
    }

    public class CRDistributedLessonUnitsByLessonReportLessonData
    {
        public int LessonUnitDistID { get; set; }
        public int CompletedQuestions { get; set; }
        public int CompletedActivities { get; set; }
        public int TotalTime { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public string LessonDesc { get; set; } = string.Empty;
        public string TargetDateOfCompletion { get; set; } = string.Empty;
        public string TargetTimeOfCompletion { get; set; } = string.Empty;
        public string TargetCompletionDateTime { get; set; } = string.Empty;
        public string Score { get; set; } = string.Empty;
        public string TotalTimeTaken { get; set; } = string.Empty;
        public string TotalTimeTakenMin { get; set; } = string.Empty;
        public string TotalTimeTakenSec { get; set; } = string.Empty;
    }
}
