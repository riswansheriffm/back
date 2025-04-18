using BackPack.Library.Responses.Report;

namespace BackPack.Library.Repositories.Interfaces.Report
{
    public interface IAllChaptersBySubjectReportRepository
    {
        Task<AllChaptersBySubjectReportResponse> AllChaptersBySubjectReportAsync(int SubjectID, int CourseID);
    }
}
