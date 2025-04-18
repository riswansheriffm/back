using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Report
{
    public class CRClassSummaryTagReportResponse : ReadBaseResponse
    {
        public CRClassSummaryTagReport Data { get; set; } = new();
    }

    public class CRClassSummaryTagReport
    {
        public CRClassSummaryTagReportData GetCRClassSummaryTagReportResult { get; set; } = new();
    }
    public class CRClassSummaryTagReportData
    {
        public List<CRClassSummaryTagReportTagData> Tags { get; set; } = new();
        public List<CRClassSummaryTagReportStudentData> StudentData { get; set; } = new();
    }

    public class CRClassSummaryTagReportTagData
    {
        public int Distribute { get; set; }
        public string TagName { get; set; } = string.Empty;
    }

    public class CRClassSummaryTagReportStudentData
    {
        public int StudentID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string LoginName { get; set; } = string.Empty;
        public List<string> StudentScore { get; set; } = new();
    }

    public class CRClassSummaryTagReportTagQueryData
    {
        public int StudentID { get; set; }
    }

    public class CRClassSummaryTagReportStudentQueryData
    {
        public int ID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string LoginName { get; set; } = string.Empty;
    }

    public class CRClassSummaryTagReportStudentTagQueryData
    {
        public string Score { get; set; } = string.Empty;
    }
}
