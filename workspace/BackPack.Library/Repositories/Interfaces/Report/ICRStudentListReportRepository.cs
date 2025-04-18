using BackPack.Library.Responses.Report;

namespace BackPack.Library.Repositories.Interfaces.Report
{
    public interface ICRStudentListReportRepository
    {
        Task<CRStudentListReportResponse> CRStudentListReportAsync(int LessonUnitDistID, int AuthorID);
    }
}
