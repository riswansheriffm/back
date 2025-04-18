using BackPack.Library.Responses.LessonPod;

namespace BackPack.Library.Services.Interfaces.LessonPod
{
    public interface ILessonFolderService
    {
        Task<LessonFoldersBySubjectResponse> GetLessonFoldersBySubjectAsync(int DomainID, int StudentID, int SubjectID, int ParentID, int ChapterID);
    }
}
