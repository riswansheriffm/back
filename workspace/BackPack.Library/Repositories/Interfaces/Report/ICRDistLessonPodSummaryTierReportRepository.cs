using BackPack.Library.Responses.Report;

namespace BackPack.Library.Repositories.Interfaces.Report
{
    public interface ICRDistLessonPodSummaryTierReportRepository
    {
        Task<CRDistLessonPodSummaryTierReportResponse> CRDistLessonPodSummaryTierReportAsync(int LoginID, int LessonUnitDistID);
    }
}
