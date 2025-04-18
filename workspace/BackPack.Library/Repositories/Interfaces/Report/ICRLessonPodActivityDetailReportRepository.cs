using BackPack.Library.Responses.Report;

namespace BackPack.Library.Repositories.Interfaces.Report
{
    public interface ICRLessonPodActivityDetailReportRepository
    {
        Task<CRLessonPodActivityDetailReportResponse> CRLessonPodActivityDetailReportAsync(int LoginID, int ContentID);
    }
}
