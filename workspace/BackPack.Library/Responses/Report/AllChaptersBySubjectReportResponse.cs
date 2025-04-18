using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Report
{
    public class AllChaptersBySubjectReportResponse : ReadBaseResponse
    {
        public AllChaptersBySubjectReportResult Data { get; set; } = new();
    }

    public class AllChaptersBySubjectReportResult
    {
        public AllChaptersBySubjectReportData GetAllChaptersBySubjectReportResult {  get; set; } = new();
    }

    public class AllChaptersBySubjectReportData
    {
        public int CompletedQuestions { get; set; }
        public int CompletedActivities { get; set; }
        public int TotalTime { get; set; }
        public int NoOfChapters { get; set; }
        public int NoOfStudents { get; set; }
        public int NoOfTeachers { get; set; }
        public string Score { get; set; } = string.Empty;
        public string TotalTimeTaken { get; set; } = string.Empty;
        public string TotalTimeTakenMin { get; set; } = string.Empty;
        public string TotalTimeTakenSec { get; set; } = string.Empty;
        public List<AllChaptersBySubjectReportChapterData> Chapters { get; set; } = new();
    }

    public class AllChaptersBySubjectReportChapterData
    {
        public int ChapterID { get; set; }
        public int SubjectID { get; set; }
        public int CompletedQuestions { get; set; }
        public int CompletedActivities { get; set; }
        public int TotalTime { get; set; }
        public int ActivityBy { get; set; }
        public string ChapterName { get; set; } = string.Empty;
        public string ChapterDesc { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public string Score { get; set; } = string.Empty;
        public string TotalTimeTaken { get; set; } = string.Empty;
        public string TotalTimeTakenMin { get; set; } = string.Empty;
        public string TotalTimeTakenSec { get; set; } = string.Empty;
    }
}
