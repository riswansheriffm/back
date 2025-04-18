using BackPack.Library.Repositories.Interfaces.LessonPod;
using BackPack.Library.Responses.LessonPod;
using BackPack.Library.Services.Interfaces.LessonPod;

namespace BackPack.Library.Services.Services.LessonPod
{
    public class LessonFolderService(
        ILessonFoldersBySubjectRepository lessonFoldersBySubjectRepository
        ) : ILessonFolderService
    {
        #region GetLessonFoldersBySubjectAsync
        public async Task<LessonFoldersBySubjectResponse> GetLessonFoldersBySubjectAsync(int DomainID, int StudentID, int SubjectID, int ParentID, int ChapterID)
        {
            LessonFoldersBySubjectResponse response = await lessonFoldersBySubjectRepository.GetLessonFoldersBySubjectAsync(DomainID, StudentID, SubjectID, ParentID, ChapterID);

            return response;
        }
        #endregion
    }
}
