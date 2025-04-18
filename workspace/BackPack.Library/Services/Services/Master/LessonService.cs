using BackPack.Library.Repositories.Interfaces.Master;
using BackPack.Library.Responses.Master.Lesson;
using BackPack.Library.Services.Interfaces.Master;

namespace BackPack.Library.Services.Services.Master
{
    public class LessonService(ILessonRepository lessonRepository) : ILessonService
    {
        #region AllLessonsForASubjectAsync
        public async Task<AllLessonsForASubjectResponse> AllLessonsForASubjectAsync(int SubjectID, int ChapterID)
        {
            AllLessonsForASubjectResponse response = await lessonRepository.AllLessonsForASubjectAsync(SubjectID, ChapterID);

            return response;
        }
        #endregion
    }
}
