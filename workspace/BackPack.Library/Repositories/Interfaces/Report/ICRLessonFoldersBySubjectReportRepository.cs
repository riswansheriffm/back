using BackPack.Library.Responses.Report;

namespace BackPack.Library.Repositories.Interfaces.Report
{
    public interface ICRLessonFoldersBySubjectReportRepository
    {
        Task<CRLessonFoldersBySubjectReportResponse> CRLessonFoldersBySubjectReportAsync(int DomainID, int AuthorID, int SubjectID, int CourseID, int ChapterID);
    }
}
