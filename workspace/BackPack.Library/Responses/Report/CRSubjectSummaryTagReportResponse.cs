using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Report
{
    public class CRSubjectSummaryTagReportResponse : ReadBaseResponse
    {
        public CRSubjectSummaryTagReport Data { get; set; } = new();
    }
    public class CRSubjectSummaryTagReport  
    {
        public CRSubjectSummaryTagReportData GetCRSubjectSummaryTagReportResult { get; set; } = new();
    }
    public class CRSubjectSummaryTagReportData
    {
        public List<CRSubjectSummaryTagReportTagData> Tags { get; set; } = new();
        public List<CRSubjectSummaryTagReportFolderData> FolderData { get; set; } = new();
    }

    public class CRSubjectSummaryTagReportTagData
    {
        public int Distribute { get; set; }
        public string TagName { get; set; } = string.Empty;
    }

    public class CRSubjectSummaryTagReportFolderData
    {
        public int FolderID { get; set; }
        public string FolderName { get; set; } = string.Empty;
        public string AverageScore { get; set; } = string.Empty;
        public List<string> Score { get; set; } = new();
        public List<string> Tags { get; set; } = new();
    }

    public class CRSubjectSummaryTagReportLessonQueryData
    {
        public int LessonID { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public string AverageScore { get; set; } = string.Empty;
        public string Score { get; set; } = string.Empty;
        public string TagName { get; set; } = string.Empty;
    }
}
