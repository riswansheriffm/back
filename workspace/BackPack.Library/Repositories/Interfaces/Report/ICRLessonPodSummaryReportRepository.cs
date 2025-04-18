using BackPack.Library.Responses.Report;

namespace BackPack.Library.Repositories.Interfaces.Report
{
    public interface ICRLessonPodSummaryReportRepository
    {
        Task<CRLessonPodSummaryReportResponse> CRLessonPodSummaryReportAsync(int LoginID, int LessonUnitDistID);
    }
}
