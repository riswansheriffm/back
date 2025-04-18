using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Report
{
    public class CRDistLessonPodSummaryTierReportResponse : ReadBaseResponse
    {
        public CRDistLessonPodSummaryTierReport Data { get; set; } = new();
    }
    public class CRDistLessonPodSummaryTierReport 
    {
        public CRDistLessonPodSummaryTierReportData GetCRDistLessonPodTierSummaryReportResult { get; set; } = new();
    }

    public class CRDistLessonPodSummaryTierReportData
    {
        public int LessonUnitDistID { get; set; } 
        public List<CRDistLessonPodSummaryTierData> TierData { get; set; } = new();
    }

    public class CRDistLessonPodSummaryTierData
    {
        public int TierStartPercentage { get; set; }
        public int TierEndPercentage { get; set; }
        public string TierName { get; set; } = string.Empty;
        public List<CRDistLessonPodSummaryTierStudentData> StudentList { get; set; } = new();
    }

    public class CRDistLessonPodSummaryTierStudentData
    {
        public int AverageScore { get; set; }
        public float TimelyCompletion { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string TimeSpent { get; set; } = string.Empty;
    }

    public class TierRangeResponse
    {
        public int MinRange { get; set; }
        public int MaxRange { get; set; }
        public string TierName { get; set; } = string.Empty;
    }

    public class CRDistLessonPodSummaryTierQueryData
    {
        public int AverageScore { get; set; }
        public float AverageTimelyCompletion { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string AverageTimeSpent { get; set; } = string.Empty;
    }
}
