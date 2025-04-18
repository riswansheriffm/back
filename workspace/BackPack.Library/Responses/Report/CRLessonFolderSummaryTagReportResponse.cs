using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Report
{
    public class CRLessonFolderSummaryTagReportResponse : ReadBaseResponse
    {
        public CRLessonFolderSummaryTagReport Data { get; set; } = new();
    }
    public class CRLessonFolderSummaryTagReport 
    {
        public CRLessonFolderSummaryTagReportData GetCRLessonFolderSummaryTagReportResult { get; set; } = new();
    }

    public class CRLessonFolderSummaryTagReportData
    {
        public List<string> TagName { get; set; } = new(); 
        public List<CRLessonFolderSummaryTagReportStudentData> StudentData { get; set; } = new();
    }

    public class CRLessonFolderSummaryTagReportStudentData
    {
        public int StudentID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string LoginName { get; set; } = string.Empty;
        public List<string> StudentScore { get; set; } = new();
    }

    public class CRLessonFolderSummaryTagReportQueryData
    {
        public int StudentID { get; set; }
        public int ID { get; set; }
        public string TagName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string LoginName { get; set; } = string.Empty;
        public string Score { get; set; } = string.Empty;
    }
}
