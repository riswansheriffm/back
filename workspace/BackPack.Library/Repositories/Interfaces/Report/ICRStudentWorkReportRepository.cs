using BackPack.Library.Responses.Report;

namespace BackPack.Library.Repositories.Interfaces.Report
{
    public interface ICRStudentWorkReportRepository
    {
        Task<CRStudentWorkReportResponse> CRStudentWorkReportAsync(int LessonUnitDistID, int StudentID, int AuthorID);
    }
}
