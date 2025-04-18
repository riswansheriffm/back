using BackPack.Library.Responses.Report;

namespace BackPack.Library.Repositories.Interfaces.Report
{
    public interface ICRSubjectSummaryTagReportRepository
    {
        Task<CRSubjectSummaryTagReportResponse> CRSubjectSummaryTagReportAsync(int SubjectID, int StudentID);
    }
}
