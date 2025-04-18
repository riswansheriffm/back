using BackPack.Library.Responses.Report;

namespace BackPack.Library.Repositories.Interfaces.Report
{
    public interface ICRChapterSummaryTagReportRepository
    {
        Task<CRChapterSummaryTagReportResponse> CRChapterSummaryTagReportAsync(int ChapterID);
    }
}
