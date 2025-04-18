using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Report
{
    public class CRLessonPodActivityDetailReportResponse : ReadBaseResponse
    {
        public CRLessonPodActivityDetailReportData Data { get; set; } = new();
    }

    public class CRLessonPodActivityDetailReportData
    {
        public int ContentID { get; set; }
        public string AppName { get; set; } = string.Empty;
        public List<string> ControlName { get; set; } = new();
        public List<CRLessonPodActivityDetailReportStudentData> StudentData { get; set; } = new();
    }

    public class CRLessonPodActivityDetailReportStudentData
    {
        public int StudentID { get; set; }
        public int ActivityStatus { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public List<float> StudentScore { get; set; } = new();
    }

    public class CRLessonPodActivityDetailReportQueryData
    {
        public string AppName { get; set; } = string.Empty;
        public string ControlName { get; set; } = string.Empty;
    }

    public class CRLessonPodActivityDetailReportStudentQueryData
    {
        public int StudentID { get; set; }
        public int ActivityStatus { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string StudentWork { get; set; } = string.Empty;
    }
}
