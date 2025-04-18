using BackPack.Library.Responses.Report;

namespace BackPack.Library.Repositories.Interfaces.Report
{
    public interface ICRLessonFolderSummaryTagReportRepository
    {
        Task<CRLessonFolderSummaryTagReportResponse> CRLessonFolderSummaryTagReportAsync(int LessonID);
    }
}
