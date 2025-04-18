using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Report
{
    public class CRStudentWorkReportResponse : ReadBaseResponse
    {
        public CRStudentWorkReportResult Data { get; set; } = new();
    }

    public class CRStudentWorkReportResult
    {
        public CRStudentWorkReportData GetCRStudentWorkResult { get; set; } = new();
    }

    public class CRStudentWorkReportData
    {
        public List<CRStudentWorkReportWorkData> LessonActivity { get; set; } = [];
        public CRStudentWorkReportLessonData LessonUnit { get; set; } = new();
    }

    public class CRStudentWorkReportWorkData
    {
        public int LessonUnitDistID { get; set; }
        public int StudentID { get; set; }
        public int ContentID { get; set; }
        public int TotalPoints { get; set; }
        public int Attempts { get; set; }
        public float Score { get; set; }
        public string ContentName { get; set; } = string.Empty;
        public string AppName { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty;
        public string TargetDateOfCompletion { get; set; } = string.Empty;
        public string TargetTimeOfCompletion { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ActualDateOfCompletion { get; set; } = string.Empty;
        public string ActualTimeOfCompletion { get; set; } = string.Empty;
        public string SpentTime { get; set; } = string.Empty;
        public string ActualCompletionDateTime { get; set; } = string.Empty;
        public string TargetCompletionDateTime { get; set; } = string.Empty;
    }

    public class CRStudentWorkReportLessonData
    {
        public string LessonName { get; set; } = string.Empty;
        public string LessonDesc { get; set; } = string.Empty;
    }
}
