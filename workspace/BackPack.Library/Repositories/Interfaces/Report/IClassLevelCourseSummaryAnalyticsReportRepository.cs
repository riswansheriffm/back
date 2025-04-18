using BackPack.Library.Responses.Report;

namespace BackPack.Library.Repositories.Interfaces.Report
{
    public interface IClassLevelCourseSummaryAnalyticsReportRepository
    {
        Task<ClassLevelCourseSummaryAnalyticsReportResponse> ClassLevelCourseSummaryAnalyticsReportAsync(int DomainID, int CourseID);
    }
}
