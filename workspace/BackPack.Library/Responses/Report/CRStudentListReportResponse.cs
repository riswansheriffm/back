using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Report
{
    public class CRStudentListReportResponse : ReadBaseResponse
    {
        public CRStudentListReportResult Data { get; set; } = new();
    }

    public class CRStudentListReportResult
    {
        public List<CRStudentListReportData> GetCRStudentListResult { get; set; } = [];
    }

    public class CRStudentListReportData
    {
        public int LessonUnitDistID { get; set; }
        public int StudentID { get; set; }
        public int CompletedActivities { get; set; }
        public int CompletedTotalScore { get; set; }
        public int CompletedStudentScore { get; set; }
        public int TotalActivities { get; set; }
        public string Status { get; set; } = string.Empty;
        public string TargetDateOfCompletion { get; set; } = string.Empty;
        public string TargetTimeOfCompletion { get; set; } = string.Empty;
        public string ActualDateOfCompletion { get; set; } = string.Empty;
        public string ActualTimeOfCompletion { get; set; } = string.Empty;
        public string LoginName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string SpentTime { get; set; } = string.Empty;
        public string TargetCompletionDateTime { get; set; } = string.Empty;
        public string ActualCompletionDateTime { get; set; } = string.Empty;
    }
}
