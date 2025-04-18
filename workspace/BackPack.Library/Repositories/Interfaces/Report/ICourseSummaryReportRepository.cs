using BackPack.Library.Responses.Report;

namespace BackPack.Library.Repositories.Interfaces.Report
{
    public interface ICourseSummaryReportRepository
    {
        Task<CourseSummaryAnalyticsReportResponse> CourseSummaryAnalyticsReportAsync(int DomainID, int CourseID, int StudentID);
    }
}
