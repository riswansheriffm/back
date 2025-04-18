using BackPack.Library.Responses.Report;

namespace BackPack.Library.Repositories.Interfaces.Report
{
    public interface ICRClassSummaryTagReportRepository
    {
        Task<CRClassSummaryTagReportResponse> CRClassSummaryTagReportAsync(int CourseID);
    }
}
