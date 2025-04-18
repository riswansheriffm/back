using BackPack.Library.Responses.Report;

namespace BackPack.Library.Repositories.Interfaces.Report
{
    public interface ILessonPodSummaryReportRepository
    {
        Task<LessonPodSummaryAnalyticsReportResponse> LessonPodSummaryAnalyticsReportAsync(int LessonUnitDistID, int StudentID);
    }
}
