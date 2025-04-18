using BackPack.Library.Responses.Report;

namespace BackPack.Library.Repositories.Interfaces.Report
{
    public interface ILessonPodActivityReportRepository
    {
        Task<LessonPodActivityAnalyticsReportResponse> LessonPodActivityAnalyticsReportAsync(int ContentID, int StudentID);
    }
}
