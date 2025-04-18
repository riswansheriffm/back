using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Report
{
    public class CRDistLessonPodTierStudentActivityReportResponse : ReadBaseResponse
    {
        public CRDistLessonPodTierStudentActivityReport Data { get; set; } = new();
    }
    public class CRDistLessonPodTierStudentActivityReport 
    {
        public CRDistLessonPodTierStudentActivityReportData GetCRDistLessonPodTierStudentActivityReportResult { get; set; } = new();
    }
    public class CRDistLessonPodTierStudentActivityReportData
    { 
        public int LessonUnitDistID { get; set; }
        public string TierName { get; set; } = string.Empty;
        public List<CRDistLessonPodTierStudentActivityReportActivityData> ActivityDetails { get; set; } = new();
    }

    public class CRDistLessonPodTierStudentActivityReportActivityData
    {
        public int ContentID { get; set; }
        public string AppName { get; set; } = string.Empty;
        public List<string> ControlName { get; set; } = new();
        public List<CRDistLessonPodTierStudentActivityReportStudentData> StudentData { get; set; } = new();
    }

    public class CRDistLessonPodTierStudentActivityReportStudentData
    {
        public int StudentID { get; set; }
        public int ActivityStatus { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public List<int> StudentScore { get; set; } = new();
    }

    public class CRDistLessonPodTierStudentActivityReportQueryData
    {
        public int StudentID { get; set; }
        public int ContentID { get; set; }
        public int ActivityStatus { get; set; }
        public string AppName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string ControlNames { get; set; } = string.Empty;
        public string StudentWork { get; set; } = string.Empty;
    }
}
