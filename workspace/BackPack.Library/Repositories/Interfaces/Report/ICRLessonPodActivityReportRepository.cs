using BackPack.Library.Responses.Report;

namespace BackPack.Library.Repositories.Interfaces.Report
{
    public interface ICRLessonPodActivityReportRepository
    {
        Task<CRLessonPodActivityReportResponse> CRLessonPodActivityReportAsync(int LoginID, int ContentID);
    }
}
