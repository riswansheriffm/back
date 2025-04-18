using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Report
{
    public class CRLessonFoldersBySubjectReportResponse : ReadBaseResponse
    {
        public CRLessonFoldersBySubjectReportResult Data { get; set; } = new();
    }

    public class CRLessonFoldersBySubjectReportResult
    {
        public CRLessonFoldersBySubjectReportData GetCRLessonFoldersBySubjectReportResult { get; set; } = new();
    }

    public class CRLessonFoldersBySubjectReportData
    {
        public int CompletedQuestions { get; set; }
        public int CompletedActivities { get; set; }
        public int TotalTime { get; set; }
        public int NoOfLessonFolders { get; set; }
        public int NoOfStudents { get; set; }
        public int NoOfTeachers { get; set; }
        public string Score { get; set; } = string.Empty;
        public string TotalTimeTaken { get; set; } = string.Empty;
        public string TotalTimeTakenMin { get; set; } = string.Empty;
        public string TotalTimeTakenSec { get; set; } = string.Empty;
        public List<CRLessonFoldersBySubjectReportLessonSummaryData> LessonFolders { get; set; } = new();
    }

    public class CRLessonFoldersBySubjectReportLessonSummaryData
    {
        public int LessonID { get; set; }
        public int LessonUnits { get; set; }
        public int CompletedQuestions { get; set; }
        public int CompletedActivities { get; set; }
        public int TotalTime { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public string LessonDesc { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public string Score { get; set; } = string.Empty;
        public string TotalTimeTaken { get; set; } = string.Empty;
        public string TotalTimeTakenMin { get; set; } = string.Empty;
        public string TotalTimeTakenSec { get; set; } = string.Empty;
    }
}
