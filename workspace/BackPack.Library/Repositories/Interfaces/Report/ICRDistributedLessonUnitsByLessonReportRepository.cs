using BackPack.Library.Responses.Report;

namespace BackPack.Library.Repositories.Interfaces.Report
{
    public interface ICRDistributedLessonUnitsByLessonReportRepository
    {
        Task<CRDistributedLessonUnitsByLessonReportResponse> CRDistributedLessonUnitsByLessonReportAsync(int AuthorID, int LessonID, int CourseID);
    }
}
