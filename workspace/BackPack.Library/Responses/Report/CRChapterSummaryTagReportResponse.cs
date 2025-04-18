using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Report
{
    public class CRChapterSummaryTagReportResponse : ReadBaseResponse
    {
        public CRChapterSummaryTagReport Data { get; set; } = new();
    }
    public class CRChapterSummaryTagReport 
    { 
        public CRChapterSummaryTagReportData GetCRChapterSummaryTagReportResult { get; set; } = new();
    }

    public class CRChapterSummaryTagReportData
    {
        public List<string> TagName { get; set; } = new();
        public List<CRChapterSummaryTagReportStudentData> StudentData { get; set; } = new();
    }

    public class CRChapterSummaryTagReportStudentData
    {
        public int StudentID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string LoginName { get; set; } = string.Empty;
        public List<string> StudentScore { get; set; } = new();
    }

    public class CRChapterSummaryTagReportQueryData
    {
        public int StudentID { get; set; }
        public int ID { get; set; }
        public string TagName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string LoginName { get; set; } = string.Empty;
        public string Score { get; set; } = string.Empty;
    }
}
